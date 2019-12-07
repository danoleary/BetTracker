module FayeLadbrokes

open System
open Domain
open DataImportHelpers

let ladbrokesId = Guid.NewGuid ()
let makeDeposit timestamp amount = makeDeposit ladbrokesId timestamp amount
let makeWithdrawal timestamp amount = makeWithdrawal ladbrokesId timestamp amount
let placeBackBet timestamp stake odds betId description = placeBackBet ladbrokesId timestamp stake odds betId description
let settleBackBet timestamp betId result = settleBackBet ladbrokesId timestamp betId result
let placeFreeBet timestamp stake odds betId description = placeFreeBet ladbrokesId timestamp stake odds betId description
let settleFreeBet timestamp betId result = settleFreeBet ladbrokesId timestamp betId result
let betId1 = Guid.NewGuid ()

let ladbrokesCommands = [
   createBookie ladbrokesId (DateTime(2019, 11, 24, 21, 0, 0)) "Ladbrokes" 
   makeDeposit (DateTime(2019, 11, 24, 21, 26, 0)) 5.0m
   placeBackBet (DateTime(2019, 11, 24, 21, 30, 0)) 5.0m 1.626m betId1 "something"
   settleBackBet (DateTime(2019, 11, 25, 21, 30, 0)) betId1 Win
   makeWithdrawal (DateTime(2019, 12, 02, 21, 26, 0)) 8.13m
]