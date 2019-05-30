module BackBetPlacedHandler

open Domain

let private subtractStakeFromBalance balance stake =
    let x = match balance with | Balance b -> b
    let y = match stake with | Stake stake -> stake
    Balance (x - y)

let applyBackBetPlaced state (backBetPlaced: CmdArgs.PlaceBackBet) =
    let newBet ={ Id = backBetPlaced.BetId; Settled = NotSettled;
                    Stake = backBetPlaced.Stake; Odds = backBetPlaced.Odds }
    let bookie = state.Bookies
                |> List.find (fun x -> x.Id = backBetPlaced.Id)
                |> (fun t -> {
                    t with Balance = subtractStakeFromBalance t.Balance backBetPlaced.Stake;
                            Bets = newBet :: t.Bets })
    let otherBookies = state.Bookies |> List.filter (fun x -> x.Id <> backBetPlaced.Id)
    { state with Bookies = bookie :: otherBookies }