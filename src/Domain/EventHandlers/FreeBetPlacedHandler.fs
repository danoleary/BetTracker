module FreeBetPlacedHandler

open Domain
open DomainHelpers

let applyFreeBetPlaced state (evt: CmdArgs.PlaceFreeBet) =
    let newBet = {
        Id = evt.BetId; Settled = NotSettled;
        Stake = evt.Stake; Odds = evt.Odds }
    let updateFunc = (fun t -> { t with Bets = newBet :: t.Bets })
    { state with Bookies = (updateBookie state.Bookies evt.Id updateFunc) }