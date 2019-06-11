module FreeBetPlacedHandler

open Domain
open DomainHelpers

let applyFreeBetPlaced state (evt: CmdArgs.PlaceFreeBet) =
    ifNotEmpty
        state
        evt
        (fun bookie evt ->
            let newBet = {
                Id = evt.BetId; State = NotSettled;
                Stake = evt.Stake; Odds = evt.Odds }
            Bookie { bookie with Bets = newBet :: bookie.Bets })