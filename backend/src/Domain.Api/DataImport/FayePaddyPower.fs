module FayePaddyPower

open System
open Domain
open DataImportHelpers

let paddyPowerId = Guid.NewGuid ()
let makeDeposit timestamp amount = makeDeposit paddyPowerId timestamp amount
let makeWithdrawal timestamp amount = makeWithdrawal paddyPowerId timestamp amount
let placeBackBet timestamp stake odds betId description = placeBackBet paddyPowerId timestamp stake odds betId description
let settleBackBet timestamp betId result = settleBackBet paddyPowerId timestamp betId result
let placeFreeBet timestamp stake odds betId description = placeFreeBet paddyPowerId timestamp stake odds betId description
let settleFreeBet timestamp betId result = settleFreeBet paddyPowerId timestamp betId result
let betId1 = Guid.NewGuid ()
let betId2 = Guid.NewGuid ()

let paddyPowerCommands = [
   createBookie paddyPowerId (DateTime(2019, 11, 17, 21, 0, 0)) "Paddy Power" 
   makeDeposit (DateTime(2019, 11, 17, 21, 26, 0)) 10.0m
   placeBackBet (DateTime(2019, 11, 17, 21, 30, 0)) 10.0m 3.3m betId1 "Buffalo Bills"
   settleBackBet (DateTime(2019, 11, 19, 21, 30, 0)) betId1 Win
   makeWithdrawal (DateTime(2019, 12, 6, 21, 26, 0)) 33.0m
]