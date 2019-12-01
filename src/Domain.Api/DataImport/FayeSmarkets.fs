module FayeSmarkets

open System
open Domain
open FSharp.Data
open System.Net.Http
open Newtonsoft.Json
open System.Text
open DataImportHelpers

let smarketsId = Guid.NewGuid ()
let smarketsCommand timestamp payload =
        { AggregateId = AggregateId smarketsId; Timestamp = timestamp; Payload = payload }
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
   smarketsCommand (DateTime(2019, 11, 17, 17, 0, 0)) (MakeDeposit { Amount = TransactionAmount 76.30m })
   smarketsCommand (DateTime(2019, 11, 17, 17, 0, 0)) (PlaceLayBet
                                                        {
                                                           Stake = Stake 9.71m;
                                                           Odds = Odds 2.08m;
                                                           BetId = BetId betId1;
                                                           EventDescription = EventDescription "Belgium Cyprus" } )
   smarketsCommand (DateTime(2019, 11, 17, 17, 15, 0)) (PlaceLayBet
                                                            {
                                                               Stake = Stake 9.77m;
                                                               Odds = Odds 1.76m;
                                                               BetId = BetId betId2;
                                                               EventDescription = EventDescription "Wales Hungary" } )
   smarketsCommand (DateTime(2019, 11, 17, 17, 30, 0)) (PlaceLayBet
                                                            {
                                                               Stake = Stake 9.78m;
                                                               Odds = Odds 2.32m;
                                                               BetId = BetId betId3;
                                                               EventDescription = EventDescription "Bordeaux Monaco" } )
   smarketsCommand (DateTime(2019, 11, 19, 17, 30, 0)) (SettleLayBet
                                                            {
                                                               BetId = BetId betId1;
                                                               Result = Win } )
   smarketsCommand (DateTime(2019, 11, 19, 21, 30, 0)) (SettleLayBet
                                                            {
                                                               BetId = BetId betId2;
                                                               Result = Lose } )
   smarketsCommand (DateTime(2019, 11, 20, 23, 57, 0)) (MakeDeposit { Amount = TransactionAmount 40.0m })
   smarketsCommand (DateTime(2019, 11, 21, 0, 0, 0)) (PlaceLayBet
                                                            {
                                                               Stake = Stake 22.30m;
                                                               Odds = Odds 5.4m;
                                                               BetId = BetId betId4;
                                                               EventDescription = EventDescription "Everton Norwich" } )
   smarketsCommand (DateTime(2019, 11, 21, 07, 0, 0)) (MakeDeposit { Amount = TransactionAmount 100.0m })
   smarketsCommand (DateTime(2019, 11, 21, 07, 52, 0)) (PlaceLayBet
                                                         {
                                                            Stake = Stake 7.92m;
                                                            Odds = Odds 5.7m;
                                                            BetId = BetId betId5;
                                                            EventDescription = EventDescription "Brentford Reading" } )
   smarketsCommand (DateTime(2019, 11, 21, 07, 55, 0)) (PlaceLayBet
                                                         {
                                                            Stake = Stake 7.14m;
                                                            Odds = Odds 4.5m;
                                                            BetId = BetId betId6;
                                                            EventDescription = EventDescription "West Ham Spurs" } )
   smarketsCommand (DateTime(2019, 11, 21, 07, 58, 0)) (PlaceLayBet
                                                         {
                                                            Stake = Stake 7.43m;
                                                            Odds = Odds 5.4m;
                                                            BetId = BetId betId7;
                                                            EventDescription = EventDescription "City Chelsea" } )
   smarketsCommand (DateTime(2019, 11, 23, 14, 25, 0)) (SettleLayBet
                                                            {
                                                               BetId = BetId betId6;
                                                               Result = Win } )
   smarketsCommand (DateTime(2019, 11, 23, 16, 25, 0)) (SettleLayBet
                                                            {
                                                               BetId = BetId betId4;
                                                               Result = Win } )
   smarketsCommand (DateTime(2019, 11, 23, 16, 59, 0)) (SettleLayBet
                                                            {
                                                               BetId = BetId betId5;
                                                               Result = Win } )

   smarketsCommand (DateTime(2019, 11, 23, 19, 24, 0)) (SettleLayBet
                                                             {
                                                                BetId = BetId betId7;
                                                                Result = Win } ) 
   smarketsCommand (DateTime(2019, 11, 24, 10, 28, 0)) (MakeWithdrawal { Amount = TransactionAmount 240.0m })  
   smarketsCommand (DateTime(2019, 11, 24, 15, 53, 0)) (SettleLayBet
                                                           {
                                                              BetId = BetId betId3;
                                                              Result = Win } )      
   smarketsCommand (DateTime(2019, 11, 24, 21, 23, 0)) (MakeDeposit { Amount = TransactionAmount 200.0m })   
   smarketsCommand (DateTime(2019, 11, 24, 21, 24, 0)) (PlaceLayBet
                                                          {
                                                             Stake = Stake 7.18m;
                                                             Odds = Odds 4.2m;
                                                             BetId = BetId betId8;
                                                             EventDescription = EventDescription "Juventus Atletico" } )
   smarketsCommand (DateTime(2019, 11, 24, 21, 26, 0)) (PlaceLayBet
                                                           {
                                                              Stake = Stake 6.99m;
                                                              Odds = Odds 4.6m;
                                                              BetId = BetId betId9;
                                                              EventDescription = EventDescription "Atalanta Zagreb" } )
   smarketsCommand (DateTime(2019, 11, 24, 21, 28, 0)) (PlaceLayBet
                                                           {
                                                              Stake = Stake 6.7m;
                                                              Odds = Odds 4.5m;
                                                              BetId = BetId betId10;
                                                              EventDescription = EventDescription "fulham derby" } )
   smarketsCommand (DateTime(2019, 11, 24, 21, 30, 0)) (PlaceLayBet
                                                            {
                                                               Stake = Stake 6.82m;
                                                               Odds = Odds 4.9m;
                                                               BetId = BetId betId11;
                                                               EventDescription = EventDescription "barca dortmund" } )
   smarketsCommand (DateTime(2019, 11, 24, 21, 42, 0)) (PlaceLayBet
                                                           {
                                                              Stake = Stake 5m;
                                                              Odds = Odds 1.65m;
                                                              BetId = BetId betId12;
                                                              EventDescription = EventDescription "fulham derby" } )
   smarketsCommand (DateTime(2019, 11, 24, 21, 51, 0)) (PlaceLayBet
                                                            {
                                                               Stake = Stake 98.46m;
                                                               Odds = Odds 1.32m;
                                                               BetId = BetId betId13;
                                                               EventDescription = EventDescription "spurs olympiakos" } )                                         
   smarketsCommand (DateTime(2019, 11, 26, 21, 43, 0)) (SettleLayBet
                                                            {
                                                               BetId = BetId betId10;
                                                               Result = Win } )  
   smarketsCommand (DateTime(2019, 11, 26, 21, 43, 0)) (SettleLayBet
                                                            {
                                                               BetId = BetId betId12;
                                                               Result = Lose } ) 
   smarketsCommand (DateTime(2019, 11, 26, 21, 51, 0)) (SettleLayBet
                                                             {
                                                                BetId = BetId betId9;
                                                                Result = Win } ) 
   smarketsCommand (DateTime(2019, 11, 26, 21, 51, 0)) (SettleLayBet
                                                             {
                                                                BetId = BetId betId13;
                                                                Result = Lose } )  
   smarketsCommand (DateTime(2019, 11, 26, 21, 52, 0)) (SettleLayBet
                                                             {
                                                                BetId = BetId betId8;
                                                                Result = Win } ) 
   smarketsCommand (DateTime(2019, 11, 26, 21, 52, 0)) (SettleLayBet
                                                             {
                                                                BetId = BetId betId11;
                                                                Result = Win } ) 
   smarketsCommand (DateTime(2019, 11, 28, 20, 57, 0)) (PlaceLayBet
                                                            {
                                                               Stake = Stake 9.22m;
                                                               Odds = Odds 3.6m;
                                                               BetId = BetId betId14;
                                                               EventDescription = EventDescription "buffalo bills" } )
   smarketsCommand (DateTime(2019, 11, 28, 21, 04, 0)) (PlaceLayBet
                                                             {
                                                                Stake = Stake 9.3m;
                                                                Odds = Odds 3.3m;
                                                                BetId = BetId betId15;
                                                                EventDescription = EventDescription "numancia" } ) 
   smarketsCommand (DateTime(2019, 11, 29, 0, 43, 0)) (SettleLayBet
                                                             {
                                                                BetId = BetId betId14;
                                                                Result = Lose } ) 
   smarketsCommand (DateTime(2019, 11, 29, 21, 50, 0)) (SettleLayBet
                                                             {
                                                                BetId = BetId betId15;
                                                                Result = Lose } ) 
   smarketsCommand (DateTime(2019, 12, 01, 14, 32, 0)) (MakeDeposit { Amount = TransactionAmount 100.0m })  
   smarketsCommand (DateTime(2019, 12, 01, 14, 33, 0)) (PlaceLayBet
                                                             {
                                                                Stake = Stake 73.06m;
                                                                Odds = Odds 4.4m;
                                                                BetId = BetId betId16;
                                                                EventDescription = EventDescription "everton leicester" } )
   smarketsCommand (DateTime(2019, 12, 01, 18, 26, 0)) (SettleLayBet
                                                             {
                                                                BetId = BetId betId16;
                                                                Result = Win } ) 
   smarketsCommand (DateTime(2019, 12, 01, 22, 15, 0)) (CreditBonus { Amount = TransactionAmount 0.07m }) // hack need to figure out multiple bets on same market
   smarketsCommand (DateTime(2019, 12, 01, 22, 16, 0)) (MakeWithdrawal { Amount = TransactionAmount 350.55m })   
   ]