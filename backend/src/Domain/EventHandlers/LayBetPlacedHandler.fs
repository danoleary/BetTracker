module LayBetPlacedHandler

open Domain
open DomainHelpers

let private subtractExposureFromBalance stake odds (Balance balance) =
    let exposure = calculateExposure stake odds
    Balance (balance - exposure)

let applyLayBetPlaced state (evt: CmdArgs.PlaceLayBet) =
    ifNotEmpty
        state
        evt
        (fun bookie evt ->
                let newBet ={ Id = evt.BetId; State = NotSettled;
                                Stake = evt.Stake; Odds = evt.Odds }

                Bookie { bookie with
                            Balance = subtractExposureFromBalance evt.Stake evt.Odds bookie.Balance;
                            Bets = newBet :: bookie.Bets } )           