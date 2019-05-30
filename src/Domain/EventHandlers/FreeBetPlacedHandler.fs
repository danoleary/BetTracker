module FreeBetPlacedHandler

open Domain

let applyFreeBetPlaced state (freeBetPlaced: CmdArgs.PlaceFreeBet) =
    let newBet = {
        Id = freeBetPlaced.BetId;Settled = NotSettled;
        Stake = freeBetPlaced.Stake; Odds = freeBetPlaced.Odds }
    let bookie = state.Bookies
                |> List.find (fun x -> x.Id = freeBetPlaced.Id)
                |> (fun t -> { t with Bets = newBet :: t.Bets })
    let otherBookies = state.Bookies |> List.filter (fun x -> x.Id <> freeBetPlaced.Id)
    { state with Bookies = bookie :: otherBookies }