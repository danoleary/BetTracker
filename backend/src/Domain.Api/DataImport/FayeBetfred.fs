module FayeBetfred

open System
open Domain
open DataImportHelpers

let betfredId = Guid.NewGuid ()
let makeDeposit timestamp amount = makeDeposit betfredId timestamp amount
let makeWithdrawal timestamp amount = makeWithdrawal betfredId timestamp amount
let placeBackBet timestamp stake odds betId description = placeBackBet betfredId timestamp stake odds betId description
let settleBackBet timestamp betId result = settleBackBet betfredId timestamp betId result
let placeFreeBet timestamp stake odds betId description = placeFreeBet betfredId timestamp stake odds betId description
let settleFreeBet timestamp betId result = settleFreeBet betfredId timestamp betId result
let betId1 = Guid.NewGuid ()
let betId2 = Guid.NewGuid ()

let betfredCommands = [
   createBookie betfredId (DateTime(2019, 11, 17, 21, 0, 0)) "Betfred" 
   makeDeposit (DateTime(2019, 11, 17, 21, 26, 0)) 10.0m
   placeBackBet (DateTime(2019, 11, 17, 21, 30, 0)) 10.0m 3.5m betId1 "Belgium Cyprus"
   settleBackBet (DateTime(2019, 11, 19, 21, 30, 0)) betId1 Lose
   placeFreeBet (DateTime(2019, 11, 17, 21, 0, 0)) 30.0m 3.5m betId2 "Everton Norwich"
   settleFreeBet (DateTime(2019, 11, 23, 18, 0, 0)) betId2 Lose
]