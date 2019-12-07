module FayeSmarkets

open System
open Domain
open DataImportHelpers

let smarketsId = Guid.NewGuid ()
let makeDeposit timestamp amount = makeDeposit smarketsId timestamp amount
let makeWithdrawal timestamp amount = makeWithdrawal smarketsId timestamp amount
let placeLayBet timestamp stake odds betId description = placeLayBet smarketsId timestamp stake odds betId description
let settleLayBet timestamp betId result = settleLayBet smarketsId timestamp betId result
let betId1 = Guid.NewGuid ()
let betId2 = Guid.NewGuid ()
let betId3 = Guid.NewGuid ()
let betId4 = Guid.NewGuid ()
let betId5 = Guid.NewGuid ()
let betId6 = Guid.NewGuid ()
let betId7 = Guid.NewGuid ()
let betId8 = Guid.NewGuid ()
let betId9 = Guid.NewGuid ()
let betId10 = Guid.NewGuid ()
let betId11 = Guid.NewGuid ()
let betId12 = Guid.NewGuid ()
let betId13 = Guid.NewGuid ()
let betId14 = Guid.NewGuid ()
let betId15 = Guid.NewGuid ()
let betId16 = Guid.NewGuid ()
let betId17 = Guid.NewGuid ()

let smarketsCommands = [
   createBookie smarketsId (DateTime(2019, 11, 17, 16, 0, 0)) "Smarkets" 
   makeDeposit (DateTime(2019, 11, 17, 17, 0, 0)) 76.30m
   placeLayBet (DateTime(2019, 11, 17, 17, 0, 0)) 9.71m 2.08m betId1 "Belgium Cyprus"
   placeLayBet (DateTime(2019, 11, 17, 17, 15, 0)) 9.77m 1.76m betId1 "Wales Hungary"
   placeLayBet (DateTime(2019, 11, 17, 17, 30, 0)) 9.78m 2.32m betId3 "Bordeaux Monaco"
   settleLayBet (DateTime(2019, 11, 19, 17, 30, 0)) betId1 Win
   settleLayBet (DateTime(2019, 11, 19, 21, 30, 0)) betId2 Lose
   makeDeposit (DateTime(2019, 11, 20, 23, 57, 0)) 40.0m
   placeLayBet (DateTime(2019, 11, 21, 0, 0, 0)) 22.30m 5.4m betId4 "Everton Norwich"
   makeDeposit (DateTime(2019, 11, 21, 07, 0, 0)) 100.0m
   placeLayBet (DateTime(2019, 11, 21, 07, 52, 0)) 7.92m 5.7m betId5 "Brentford Reading"
   placeLayBet (DateTime(2019, 11, 21, 07, 55, 0)) 7.14m 4.5m betId6 "West Ham Spurs"
   placeLayBet (DateTime(2019, 11, 21, 07, 58, 0)) 7.43m 5.4m betId7 "City Chelsea"
   settleLayBet (DateTime(2019, 11, 23, 14, 25, 0)) betId6 Win
   settleLayBet (DateTime(2019, 11, 23, 16, 25, 0)) betId4 Win
   settleLayBet (DateTime(2019, 11, 23, 16, 59, 0)) betId5 Win
   settleLayBet (DateTime(2019, 11, 23, 19, 24, 0)) betId7 Win
   makeWithdrawal (DateTime(2019, 11, 24, 10, 28, 0)) 240.0m
   settleLayBet (DateTime(2019, 11, 24, 15, 53, 0)) betId3 Win
   makeDeposit (DateTime(2019, 11, 24, 21, 23, 0)) 200.0m
   placeLayBet (DateTime(2019, 11, 24, 21, 24, 0)) 7.18m 4.2m betId8 "Juventus Atletico"
   placeLayBet (DateTime(2019, 11, 24, 21, 26, 0)) 6.99m 4.6m betId9 "Atalanta Zagreb"
   placeLayBet (DateTime(2019, 11, 24, 21, 28, 0)) 6.7m 4.5m betId10 "fulham derby"
   placeLayBet (DateTime(2019, 11, 24, 21, 30, 0)) 6.82m 4.9m betId11 "barca dortmund"
   placeLayBet (DateTime(2019, 11, 24, 21, 42, 0)) 5m 1.65m betId12 "fulham derby"
   placeLayBet (DateTime(2019, 11, 24, 21, 51, 0)) 98.46m 1.32m betId13 "spurs olympiakos"                                      
   settleLayBet (DateTime(2019, 11, 26, 21, 43, 0)) betId10 Win
   settleLayBet (DateTime(2019, 11, 26, 21, 43, 0)) betId12 Lose
   settleLayBet (DateTime(2019, 11, 26, 21, 51, 0)) betId9 Win
   settleLayBet (DateTime(2019, 11, 26, 21, 51, 0)) betId13 Lose
   settleLayBet (DateTime(2019, 11, 26, 21, 52, 0)) betId8 Win
   settleLayBet (DateTime(2019, 11, 26, 21, 52, 0)) betId11 Win
   placeLayBet (DateTime(2019, 11, 28, 20, 57, 0)) 9.22m 3.6m betId14 "buffalo bills"
   placeLayBet (DateTime(2019, 11, 28, 21, 04, 0)) 9.3m 3.3m betId15 "numancia"
   settleLayBet (DateTime(2019, 11, 29, 0, 43, 0)) betId14 Lose
   settleLayBet (DateTime(2019, 11, 29, 21, 50, 0)) betId15 Lose
   makeDeposit (DateTime(2019, 12, 01, 14, 32, 0)) 100.0m
   placeLayBet (DateTime(2019, 12, 01, 14, 33, 0)) 73.06m 4.4m betId16 "everton leicester"
   settleLayBet (DateTime(2019, 12, 01, 18, 26, 0)) betId16 Win
   creditBonus smarketsId (DateTime(2019, 12, 01, 22, 15, 0)) 3.08m // hack need to figure out multiple bets on same market
   makeWithdrawal (DateTime(2019, 12, 01, 22, 16, 0)) 350.55m
]