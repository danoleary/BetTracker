open System
open Domain
open Domain.CmdArgs
open FSharp.Data

let eventStore = CommandHandler.createDemoStore CommandHandler.StorageType.InMemory

let pipeline cmd =
    cmd
    |> CommandHandler.handle eventStore
    |> List.iter ReadSide.handle

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
                                            Transaction = { Timestamp = DateTime.UtcNow; Amount = TransactionAmount (decimal x.Amount) } }})

    let withdrawalCommands =
        Withdrawals
            .Load("https://docs.google.com/spreadsheets/d/1ofH2vj_uaiwcDG_26DFyln5RwkXwTgjSJJVsLUfrbNg/export?exportFormat=csv")
            .Rows
        |> Seq.map (fun x -> { 
                                AggregateId = AggregateId x.Id;
                                Timestamp = DateTime.Parse(x.Timestamp);
                                Payload = MakeWithdrawal {
                                            Transaction = { Timestamp = DateTime.UtcNow; Amount = TransactionAmount x.Amount } }})

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

    let cashoutbackbets =
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

    let allCommands =
        Seq.concat [addBookieCommands; depositCommands; placeBackBetCommands; settleBackBetCommands;
                    withdrawalCommands; placeFreeBetCommands; settleFreeBetCommands; cashoutbackbets ]                
        |> Seq.filter (fun x -> 
            let (AggregateId aggId) = x.AggregateId 
            (List.contains aggId includedBookies))
        |> Seq.sortBy (fun x -> x.Timestamp)

    Seq.iter (fun x -> printfn "%A, %A" x.Timestamp (x.Payload.GetType ()))  allCommands          

    Seq.iter (fun x -> x |> pipeline) allCommands

    printState "After"
    0