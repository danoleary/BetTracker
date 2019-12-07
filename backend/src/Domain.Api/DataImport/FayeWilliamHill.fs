module FayeWilliamHill

open System
open Domain
open DataImportHelpers

let williamHillId = Guid.NewGuid ()
let makeDeposit timestamp amount = makeDeposit williamHillId timestamp amount
let makeWithdrawal timestamp amount = makeWithdrawal williamHillId timestamp amount
let placeBackBet timestamp stake odds betId description = placeBackBet williamHillId timestamp stake odds betId description
let settleBackBet timestamp betId result = settleBackBet williamHillId timestamp betId result
let placeFreeBet timestamp stake odds betId description = placeFreeBet williamHillId timestamp stake odds betId description
let settleFreeBet timestamp betId result = settleFreeBet williamHillId timestamp betId result
let betId1 = Guid.NewGuid ()
let betId2 = Guid.NewGuid ()
let betId3 = Guid.NewGuid ()
let betId4 = Guid.NewGuid ()

let willaimHillCommands = [
   createBookie williamHillId (DateTime(2019, 11, 17, 21, 0, 0)) "William Hill" 
   makeDeposit (DateTime(2019, 11, 17, 21, 26, 0)) 10.0m
   placeBackBet (DateTime(2019, 11, 17, 21, 30, 0)) 10.0m 1.7m betId1 "Wales Hungary"
   settleBackBet (DateTime(2019, 11, 19, 21, 30, 0)) betId1 Win
   placeFreeBet (DateTime(2019, 11, 21, 7, 52, 0)) 10.0m 5.5m betId2 "Brentford Reading"
   settleFreeBet (DateTime(2019, 11, 23, 18, 0, 0)) betId2 Lose
   placeFreeBet (DateTime(2019, 11, 21, 7, 52, 0)) 10.0m 4.2m betId3 "West Ham Spurs"
   settleFreeBet (DateTime(2019, 11, 23, 18, 0, 0)) betId3 Lose
   placeFreeBet (DateTime(2019, 11, 21, 7, 52, 0)) 10.0m 5.0m betId4 "City Chelsea"
   settleFreeBet (DateTime(2019, 11, 23, 18, 0, 0)) betId4 Lose
   makeWithdrawal (DateTime(2019, 11, 24, 21, 26, 0)) 17.0m
]