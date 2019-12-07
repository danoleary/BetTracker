module FayeSky

open System
open Domain
open DataImportHelpers

let skyId = Guid.NewGuid ()
let makeDeposit timestamp amount = makeDeposit skyId timestamp amount
let makeWithdrawal timestamp amount = makeWithdrawal skyId timestamp amount
let placeBackBet timestamp stake odds betId description = placeBackBet skyId timestamp stake odds betId description
let settleBackBet timestamp betId result = settleBackBet skyId timestamp betId result
let placeFreeBet timestamp stake odds betId description = placeFreeBet skyId timestamp stake odds betId description
let settleFreeBet timestamp betId result = settleFreeBet skyId timestamp betId result
let betId1 = Guid.NewGuid ()
let betId2 = Guid.NewGuid ()
let betId3 = Guid.NewGuid ()
let betId4 = Guid.NewGuid ()
let betId5 = Guid.NewGuid ()

let skyCommands = [
   createBookie skyId (DateTime(2019, 11, 17, 21, 0, 0)) "Skybet" 
   makeDeposit (DateTime(2019, 11, 17, 21, 26, 0)) 10.0m
   placeBackBet (DateTime(2019, 11, 17, 21, 48, 0)) 10.0m 2.25m betId1 "Bordeaux Monaco"
   settleBackBet (DateTime(2019, 11, 24, 15, 53, 0)) betId1 Lose
   placeFreeBet (DateTime(2019, 11, 24, 21, 48, 0)) 10.0m 4.0m betId2 "Fulham Derby"
   settleFreeBet (DateTime(2019, 11, 26, 18, 0, 0)) betId2 Lose
   placeFreeBet (DateTime(2019, 11, 24, 21, 48, 0)) 10.0m 4.2m betId3 "Atalanta Zagreb"
   settleFreeBet (DateTime(2019, 11, 26, 18, 0, 0)) betId3 Lose
   placeFreeBet (DateTime(2019, 11, 24, 21, 48, 0)) 10.0m 4.2m betId4 "Juventus Atletico"
   settleFreeBet (DateTime(2019, 11, 26, 18, 0, 0)) betId4 Lose
   placeFreeBet (DateTime(2019, 11, 24, 21, 48, 0)) 10.0m 4.33m betId5 "Barca Dortmund"
   settleFreeBet (DateTime(2019, 11, 26, 18, 0, 0)) betId5 Lose
]