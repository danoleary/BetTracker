module FayeUnibet

open System
open Domain
open DataImportHelpers

let unibetId = Guid.NewGuid ()
let makeDeposit timestamp amount = makeDeposit unibetId timestamp amount
let makeWithdrawal timestamp amount = makeWithdrawal unibetId timestamp amount
let placeBackBet timestamp stake odds betId description = placeBackBet unibetId timestamp stake odds betId description
let settleBackBet timestamp betId result = settleBackBet unibetId timestamp betId result
let placeFreeBet timestamp stake odds betId description = placeFreeBet unibetId timestamp stake odds betId description
let settleFreeBet timestamp betId result = settleFreeBet unibetId timestamp betId result
let betId1 = Guid.NewGuid ()
let betId2 = Guid.NewGuid ()

let unibetCommands = [
   createBookie unibetId (DateTime(2019, 12, 7, 11, 0, 0)) "Unibet" 
   makeDeposit (DateTime(2019, 12, 7, 11, 5, 0)) 40.0m
   placeBackBet (DateTime(2019, 12, 7, 11, 10, 0)) 40.0m 5.15m betId1 "Spurs Burnley draw"
]