module LayBetPlacedHandler

open Domain
open DomainHelpers

let private subtractExposureFromBalance stake odds (Balance balance) =
    let exposure = calculateExposure stake odds
    Balance (balance - exposure)

let applyLayBetPlaced state (evt: CmdArgs.PlaceLayBet) =
    let newBet ={ Id = evt.BetId; State = NotSettled;
                    Stake = evt.Stake; Odds = evt.Odds }

    let updateFunc = (fun t -> { t with
                                    Balance = subtractExposureFromBalance evt.Stake evt.Odds t.Balance;
                                    Bets = newBet :: t.Bets })
    { state with Bookies =
                    (updateBookie
                        state.Bookies
                        evt.Id
                        updateFunc) }