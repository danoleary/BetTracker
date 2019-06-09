open System
open Domain
open Domain.CmdArgs
open FSharp.Data
open System.Globalization

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
            .Load("/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/bookies.csv")
            .Rows
        |> Seq.map (
            fun x -> {  AggregateId = AggregateId x.Id;
                        Timestamp = DateTime.Today.AddYears(-10);
                        Payload = AddBookie {
                                    Id = BookieId x.Id;
                                    Name = x.Name }})
                            
    let depositCommands =
        Deposits
            .Load("/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/deposits.csv")
            .Rows
        |> Seq.map (fun x -> {
                                AggregateId = AggregateId x.Id;
                                Timestamp = DateTime.Parse(x.Timestamp);
                                Payload = MakeDeposit {
                                            Id = BookieId x.Id;
                                            Transaction = { Timestamp = DateTime.UtcNow; Amount = TransactionAmount (decimal x.Amount) } }})

    let withdrawalCommands =
        Withdrawals
            .Load("/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/withdrawals.csv")
            .Rows
        |> Seq.map (fun x -> { 
                                AggregateId = AggregateId x.Id;
                                Timestamp = DateTime.Parse(x.Timestamp);
                                Payload = MakeWithdrawal {
                                            Id = BookieId x.Id;
                                            Transaction = { Timestamp = DateTime.UtcNow; Amount = TransactionAmount x.Amount } }})

    let placeBackBetCommands =
        PlaceBackBets
            .Load("/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/placebackbets.csv")
            .Rows
        |> Seq.map (fun x -> {
                                AggregateId = AggregateId x.BookieId;
                                Timestamp = DateTime.Parse(x.``Date placed``);
                                Payload = PlaceBackBet {
                                            Id = BookieId x.BookieId;
                                            Stake =  Stake x.Stake;
                                            Odds = Odds x.Odds;
                                            BetId = BetId x.BetId }})

    let settleBackBetCommands =
        SettleBackBets
            .Load("/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/settlebackbets.csv")
            .Rows
        |> Seq.map (fun x -> {
                                AggregateId = AggregateId x.BookieId;
                                Timestamp = DateTime.Parse(x.``Date settled``);
                                Payload = SettleBackBet {
                                            Id = BookieId x.BookieId;
                                            Result = if x.Win then Win else Lose;
                                            BetId = BetId x.BetId }})

    let placeFreeBetCommands =
        PlaceFreeBets
            .Load("/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/placefreebets.csv")
            .Rows
        |> Seq.map (fun x -> {
                                AggregateId = AggregateId x.BookieId;
                                Timestamp = DateTime.Parse(x.``Date placed``);
                                Payload = PlaceFreeBet {
                                            Id = BookieId x.BookieId;
                                            Stake =  Stake x.Stake;
                                            Odds = Odds x.Odds;
                                            BetId = BetId x.BetId }})

    let settleFreeBetCommands =
        SettleBackBets
            .Load("/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/settlefreebets.csv")
            .Rows
        |> Seq.map (fun x -> {
                                AggregateId = AggregateId x.BookieId;
                                Timestamp = DateTime.Parse(x.``Date settled``);
                                Payload = SettleFreeBet {
                                            Id = BookieId x.BookieId;
                                            Result = if x.Win then Win else Lose;
                                            BetId = BetId x.BetId }})

    let cashoutbackbets =
        CashOutBackBets
            .Load("/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/cashoutbackbets.csv")
            .Rows
        |> Seq.map (fun x -> 
                                {
                                AggregateId = AggregateId x.BookieId
                                Timestamp = new DateTime(2019, 06, 03, 20, 43, 0)
                                Payload = CashOutBackBet {
                                            Id = BookieId x.BookieId;
                                            CashOutAmount = CashOutAmount x.CashOutAmount;
                                            BetId = BetId x.BetId }})

    let allCommands =
        // Seq.concat [addBookieCommands; depositCommands; withdrawalCommands; placeBackBetCommands;
        //             settleBackBetCommands]
        Seq.concat [addBookieCommands; depositCommands; placeBackBetCommands; settleBackBetCommands;
                    withdrawalCommands; placeFreeBetCommands; settleFreeBetCommands; cashoutbackbets ]                
        |> Seq.filter (fun x -> 
            let (AggregateId aggId) = x.AggregateId 
            (List.contains aggId includedBookies))
        |> Seq.sortBy (fun x -> x.Timestamp)

    Seq.iter (fun x -> printfn "%A, %A" x.Timestamp (x.Payload.GetType ()))  allCommands          

    Seq.iter (fun x -> x |> pipeline) allCommands

    printState "After task added"
    0