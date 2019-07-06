open System
open Domain
open Domain.CmdArgs
open FSharp.Data

let eventStore = CommandHandler.createDemoStore CommandHandler.StorageType.InMemory

let pipeline cmd =
    let r = cmd |> CommandHandler.handle eventStore
    match r with
    | Ok x -> List.iter ReadSide.handle x
    | _ -> ()

let includedBookies = [
    Guid.Parse("436a7be7-7ecc-49ee-8592-8b1309d231c7") //betfred
    Guid.Parse("f2c0ad88-525d-4f5c-8629-ba64ce1eb7b2") // william hill
    Guid.Parse("8fc21e32-0e85-4047-a2fe-51b5a8f6c911") //moplay
    Guid.Parse("5b0c957a-6d0c-4003-be12-66c7fd9350ba") //netbet
    Guid.Parse("d0c83809-5614-4d10-b05d-9f4da75fc8c1") //k8
    Guid.Parse("1478766f-8480-46a7-8134-d35a3d64017f") //comeon
    Guid.Parse("ac133ab6-99b8-4690-880d-50b092f12c78") //betvictor need to add in cash out event
    Guid.Parse("904c9199-8b81-4a96-af9e-c7d83317ab0c") //boylesports
    Guid.Parse("56f01b6b-7545-463a-bbe0-224963b844ae") //betuk
    Guid.Parse("5987a4a8-8797-4f08-bb6d-20290f26ab6d") //888sport
    Guid.Parse("ce7042e9-9b50-4690-a1f9-8b079c7430d1") //laba360
    Guid.Parse("41c661ad-1dd5-4730-9ab1-63fd1427e22b") //7 best bets
    Guid.Parse("41ad0a7c-0bfb-41ae-aa3c-42d50433f3cb") // 21bet
    Guid.Parse("234f675e-5314-4d5b-a78c-318ed7e64839") //the pools
    Guid.Parse("eda560dc-de49-4593-a616-2cebc48c95e9") //bwin
    Guid.Parse("4e1e0488-1dd7-4cf4-b144-7178539e4edd") //betway
    Guid.Parse("144dca29-b091-46ab-8051-01ed9b707924") //expekt
    Guid.Parse("8404c586-8299-443d-8762-6c001206d935") //138.com
    Guid.Parse("9153c15c-2a0c-41fd-8b91-ec0491475b35") //betstars
    Guid.Parse("e22d4048-eab0-407e-bca7-81cb9ee7ca39") //jenningsbet
    Guid.Parse("49405d6b-925a-413a-96e7-62dd721386f4") //blacktype
    Guid.Parse("b14a8f3f-b5d0-4b25-9661-8ffb6aa84304") //dafabet
    Guid.Parse("fabce6ca-52c4-4638-893f-d8f54d5d46fe") //12bet
    Guid.Parse("2a1a9de1-f184-4da2-b87c-e5b48e8169db") //ladbrokes
    Guid.Parse("5eff672c-350c-41c0-b1b0-f485b18b36ea") //totesport
    Guid.Parse("cb3b6171-b957-4e41-9860-4308de03b093") //sportingbet
    Guid.Parse("be55614c-1b25-4c95-a8e7-8146471470fa") //starsports
    Guid.Parse("160e8048-4830-46f5-959d-893a46e24052") //bet365
    Guid.Parse("be62db7c-2481-4eef-a685-203fcf86af71") //unibet
    Guid.Parse("c4564c9d-22a8-49b4-b30d-455ad82cbfcb") //fansbet
    Guid.Parse("f4b368b5-d546-4030-a5c5-0647771602ac") //mansionbet
    Guid.Parse("03ace3c2-51b9-4511-8cb4-8ae61fb1fafd") //10bet
    Guid.Parse("5d9cb0b5-8ed4-4b5f-b5cc-096670ff0dd4") //paddypower
    Guid.Parse("fb6fcbb2-4222-487e-86f8-e269ec962538") //vbet
    Guid.Parse("96b35237-1129-4bdd-81f1-1cb593aa688d") //mobilebet
    Guid.Parse("fc8ed4ef-acfb-4ac2-877e-81eed86d1b1e") //novibet
    Guid.Parse("28d6d57c-046d-4157-ae66-e7ba9fafbb78") //mr green
    Guid.Parse("52a2d5b9-a570-4841-b5e0-622a3c41b894") //royal panda
    Guid.Parse("1c29d6dc-ad55-4735-8549-846e291ac634") //m88
    Guid.Parse("83328743-6a9c-42ea-b605-d6d8dcefc775") //bet at home
    Guid.Parse("b0f87a6a-4cda-4410-a2df-b437a0ae99a1") //marathon
    Guid.Parse("37732383-57c7-4ef1-810b-fc10dff68490") //skybet
    Guid.Parse("f79c95f8-939f-4cf4-8d1b-e36e11378e93") //betsafe
    Guid.Parse("6cb47278-300d-415f-8ffd-15205886d989") //coral
    Guid.Parse("89a69c43-d3c0-43ee-8e35-0fb87ba687ed") //betvision
    Guid.Parse("14a57318-1a32-4131-a6db-080846920684") //energybet
    //Guid.Parse("b239164f-e52b-4577-9af3-727ea91257cc") //betfair //TODO bets
    Guid.Parse("3a67dee0-d855-48b1-a25f-b7ceead5525e") //smarkets //TODO bets
]

let printState (desc:string) =
    List.iter
        (fun x -> eventStore.GetCurrentState (AggregateId x) |> printfn "[%s] %A" (desc.ToUpper()))
        includedBookies

type Bookies =
    CsvProvider<"/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/bookies.csv">
type Deposits =
    CsvProvider<"/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/deposits.csv">
type Withdrawals =
    CsvProvider<"/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/withdrawals.csv">
type PlaceBackBets =
    CsvProvider<"/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/placebackbets.csv">
type SettleBackBets =
    CsvProvider<"/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/settlebackbets.csv">
type PlaceFreeBets =
    CsvProvider<"/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/placefreebets.csv">
type SettleFreeBets =
    CsvProvider<"/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/settlefreebets.csv">
type CashOutBackBets =
    CsvProvider<"/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/cashoutbackbets.csv">
type PlaceLayBets =
    CsvProvider<"/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/placelaybets.csv">
type SettleLayBets =
    CsvProvider<"/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/settlelaybets.csv">
type CreditBonuses =
    CsvProvider<"/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/creditbonuses.csv">



[<EntryPoint>]
let main argv =

    let addBookieCommands =
        Bookies
            .Load("https://docs.google.com/spreadsheets/d/13AXqyemjgob_DoqN84jUxnrJDHqcBtuGvHAchjR5BMI/export?exportFormat=csv")
            .Rows
        |> Seq.map (
            fun x -> {  AggregateId = AggregateId x.Id;
                        Timestamp = DateTime.Today.AddYears(-10);
                        Payload = AddBookie {
                                    BookieId = BookieId x.Id;
                                    Name = x.Name }})
                            
    let depositCommands =
        Deposits
            .Load("https://docs.google.com/spreadsheets/d/1vK_8HtQHRDSpoMQmapPQY6kg3JHVfFDCoNLFNg6Ck8s/export?exportFormat=csv")
            .Rows
        |> Seq.map (fun x -> {
                                AggregateId = AggregateId x.Id;
                                Timestamp = DateTime.Parse(x.Timestamp);
                                Payload = MakeDeposit {
                                            Transaction = TransactionAmount (decimal x.Amount) } })

    let withdrawalCommands =
        Withdrawals
            .Load("https://docs.google.com/spreadsheets/d/1ofH2vj_uaiwcDG_26DFyln5RwkXwTgjSJJVsLUfrbNg/export?exportFormat=csv")
            .Rows
        |> Seq.map (fun x -> { 
                                AggregateId = AggregateId x.Id;
                                Timestamp = DateTime.Parse(x.Timestamp);
                                Payload = MakeWithdrawal {
                                            Transaction = TransactionAmount x.Amount } })

    let placeBackBetCommands =
        PlaceBackBets
            .Load("https://docs.google.com/spreadsheets/d/1xhHtOQNb1mm-r-UWFHxdiK4tewjXPrLKPz6P1dvXcfE/export?exportFormat=csv")
            .Rows
        |> Seq.map (fun x -> {
                                AggregateId = AggregateId x.BookieId;
                                Timestamp = DateTime.Parse(x.``Date placed``);
                                Payload = PlaceBackBet {
                                            Stake =  Stake x.Stake;
                                            Odds = Odds x.Odds;
                                            BetId = BetId x.BetId }})

    let settleBackBetCommands =
        SettleBackBets
            .Load("https://docs.google.com/spreadsheets/d/1TlZ6aPSJRyW_5-YSbNp9KQ_kEarFUrue9t9HtPbFdDY/export?exportFormat=csv")
            .Rows
        |> Seq.map (fun x -> {
                                AggregateId = AggregateId x.BookieId;
                                Timestamp = DateTime.Parse(x.``Date settled``);
                                Payload = SettleBackBet {
                                            Result = if x.Win then Win else Lose;
                                            BetId = BetId x.BetId }})

    let placeFreeBetCommands =
        PlaceFreeBets
            .Load("https://docs.google.com/spreadsheets/d/1StGsS86PuitnzfgCxipWUIIOJU2kr5Aq6uwL6eJXOPk/export?exportFormat=csv")
            .Rows
        |> Seq.map (fun x -> {
                                AggregateId = AggregateId x.BookieId;
                                Timestamp = DateTime.Parse(x.``Date placed``);
                                Payload = PlaceFreeBet {
                                            Stake =  Stake x.Stake;
                                            Odds = Odds x.Odds;
                                            BetId = BetId x.BetId }})

    let settleFreeBetCommands =
        SettleBackBets
            .Load("https://docs.google.com/spreadsheets/d/1aM7Kz2EFm0Iy3f_k-tP79xW-AyC136kHDT3qir54XCo/export?exportFormat=csv")
            .Rows
        |> Seq.map (fun x -> {
                                AggregateId = AggregateId x.BookieId;
                                Timestamp = DateTime.Parse(x.``Date settled``);
                                Payload = SettleFreeBet {
                                            Result = if x.Win then Win else Lose;
                                            BetId = BetId x.BetId }})

    let cashoutbackbetCommands =
        CashOutBackBets
            .Load("https://docs.google.com/spreadsheets/d/1yJGP-7Rx2yrV1FGz7zzWOZK6AScCOqUtyVHELKpQ0mE/export?exportFormat=csv")
            .Rows
        |> Seq.map (fun x -> 
                                {
                                AggregateId = AggregateId x.BookieId
                                Timestamp = new DateTime(2019, 06, 03, 20, 43, 0)
                                Payload = CashOutBackBet {
                                            CashOutAmount = CashOutAmount x.CashOutAmount;
                                            BetId = BetId x.BetId }})
                                           
    let placelaybetCommands =
        PlaceLayBets
            .Load("https://docs.google.com/spreadsheets/d/1pyMZ4cdRcUF99v-tGAk8Y-JaP06BPv6V5cTyAhCWVqE/export?exportFormat=csv")
            .Rows
        |> Seq.map (fun x -> 
                                {
                                AggregateId = AggregateId x.BookieId
                                Timestamp = DateTime.Parse(x.``Date placed``)
                                Payload = PlaceLayBet {
                                            Stake =  Stake x.Stake;
                                            Odds = Odds x.Odds;
                                            BetId = BetId x.BetId }})   

    let settlelaybetCommands =
        SettleLayBets
            .Load("https://docs.google.com/spreadsheets/d/1gM_CAmsAUBjmI4bvG6nc9kEhig-O44xzXPlXxY7Ke5M/export?exportFormat=csv")
            .Rows
        |> Seq.map (fun x -> 
                                {
                                AggregateId = AggregateId x.BookieId
                                Timestamp = DateTime.Parse(x.``Date settled``)
                                Payload = SettleLayBet {
                                            Result = if x.Win then Win else Lose;
                                            BetId = BetId x.BetId }})

    let creditBonusCommands =
        CreditBonuses
            .Load("/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/creditbonuses.csv")
            .Rows
        |> Seq.map (fun x -> 
                                {
                                AggregateId = AggregateId x.BookieId
                                Timestamp = DateTime.Parse(x.Timestamp)
                                Payload = CreditBonus 
                                            { Amount = TransactionAmount x.Amount  }})                       

    let allCommands =
        Seq.concat [addBookieCommands; depositCommands; placeBackBetCommands; settleBackBetCommands;
                    withdrawalCommands; placeFreeBetCommands; settleFreeBetCommands; cashoutbackbetCommands;
                    creditBonusCommands; placelaybetCommands; settlelaybetCommands ]                
        |> Seq.filter (fun x -> 
            let (AggregateId aggId) = x.AggregateId 
            (List.contains aggId includedBookies))
        |> Seq.sortBy (fun x -> x.Timestamp)       

    allCommands
    |> Seq.iter (fun x -> x |> pipeline)

    printState "After"
    0