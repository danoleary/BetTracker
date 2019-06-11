module BackBetPlacedHandler

open Domain
open DomainHelpers

let applyBackBetPlaced state (evt: CmdArgs.PlaceBackBet) =
        ifNotEmpty
                state
                evt
                (fun bookie evt ->
                        let newBet ={ Id = evt.BetId; State = NotSettled;
                                        Stake = evt.Stake; Odds = evt.Odds }

                        Bookie { bookie with
                                    Balance = subtractStakeFromBalance bookie.Balance evt.Stake;
                                    Bets = newBet :: bookie.Bets })
   

