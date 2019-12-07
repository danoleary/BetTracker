module FayeBetway

open System
open Domain
open DataImportHelpers

let betwayId = Guid.NewGuid ()
let makeDeposit timestamp amount = makeDeposit betwayId timestamp amount
let makeWithdrawal timestamp amount = makeWithdrawal betwayId timestamp amount
let placeBackBet timestamp stake odds betId description = placeBackBet betwayId timestamp stake odds betId description
let settleBackBet timestamp betId result = settleBackBet betwayId timestamp betId result
let placeFreeBet timestamp stake odds betId description = placeFreeBet betwayId timestamp stake odds betId description
let settleFreeBet timestamp betId result = settleFreeBet betwayId timestamp betId result
let betId1 = Guid.NewGuid ()
let betId2 = Guid.NewGuid ()

let betwayCommands = [
   createBookie betwayId (DateTime(2019, 12, 7, 11, 0, 0)) "Betway" 
   makeDeposit (DateTime(2019, 12, 7, 11, 5, 0)) 30.0m
   placeBackBet (DateTime(2019, 12, 7, 11, 10, 0)) 30.0m 1.85m betId1 "City Utd both to score"
]