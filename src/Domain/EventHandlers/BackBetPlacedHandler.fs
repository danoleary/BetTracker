module BackBetPlacedHandler

open Domain
open DomainHelpers

let applyBackBetPlaced state (evt: CmdArgs.PlaceBackBet) =
    let newBet ={ Id = evt.BetId; State = NotSettled;
                    Stake = evt.Stake; Odds = evt.Odds }

    let updateFunc = (fun t -> { t with
                                    Balance = subtractStakeFromBalance t.Balance evt.Stake;
                                    Bets = newBet :: t.Bets })
    { state with Bookies =
                    (updateBookie
                        state.Bookies
                        evt.Id
                        updateFunc) }