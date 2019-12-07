module FayeFansbet

open System
open Domain
open DataImportHelpers

let fansbetId = Guid.NewGuid ()
let makeDeposit timestamp amount = makeDeposit fansbetId timestamp amount
let makeWithdrawal timestamp amount = makeWithdrawal fansbetId timestamp amount
let placeBackBet timestamp stake odds betId description = placeBackBet fansbetId timestamp stake odds betId description
let settleBackBet timestamp betId result = settleBackBet fansbetId timestamp betId result
let placeFreeBet timestamp stake odds betId description = placeFreeBet fansbetId timestamp stake odds betId description
let settleFreeBet timestamp betId result = settleFreeBet fansbetId timestamp betId result
let betId1 = Guid.NewGuid ()

let fansbetCommands = [
   createBookie fansbetId (DateTime(2019, 11, 28, 21, 0, 0)) "Fansbet" 
   makeDeposit (DateTime(2019, 11, 28, 21, 1, 0)) 10.0m
   placeBackBet (DateTime(2019, 11, 28, 21, 30, 0)) 10.0m 3.05m betId1 "something"
   settleBackBet (DateTime(2019, 11, 29, 21, 30, 0)) betId1 Win
   creditBonus fansbetId (DateTime(2019, 12, 1, 20, 15, 0)) 9.5m
   makeWithdrawal (DateTime(2019, 12, 1, 21, 1, 0)) 40.0m
]