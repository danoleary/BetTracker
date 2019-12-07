module FayeBet365

open System
open Domain
open DataImportHelpers

let bet365Id = Guid.NewGuid ()
let makeDeposit timestamp amount = makeDeposit bet365Id timestamp amount
let makeWithdrawal timestamp amount = makeWithdrawal bet365Id timestamp amount
let placeBackBet timestamp stake odds betId description = placeBackBet bet365Id timestamp stake odds betId description
let settleBackBet timestamp betId result = settleBackBet bet365Id timestamp betId result
let placeFreeBet timestamp stake odds betId description = placeFreeBet bet365Id timestamp stake odds betId description
let settleFreeBet timestamp betId result = settleFreeBet bet365Id timestamp betId result
let betId1 = Guid.NewGuid ()
let betId2 = Guid.NewGuid ()

let bet365Commands = [
   createBookie bet365Id (DateTime(2019, 11, 24, 21, 0, 0)) "Bet365" 
   makeDeposit (DateTime(2019, 11, 24, 21, 26, 0)) 100.0m
   placeBackBet (DateTime(2019, 11, 24, 21, 30, 0)) 100.0m 1.2858m betId1 "Spurs"
   settleBackBet (DateTime(2019, 11, 25, 21, 30, 0)) betId1 Win
   placeFreeBet (DateTime(2019, 11, 27, 21, 0, 0)) 100.0m 4.2m betId2 "Draw"
   settleFreeBet (DateTime(2019, 11, 28, 18, 0, 0)) betId2 Lose
   makeWithdrawal (DateTime(2019, 11, 28, 20, 45, 0)) 128.58m
]