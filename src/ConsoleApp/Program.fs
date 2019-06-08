open System
open Domain
open Domain.CmdArgs
open FSharp.Data

let eventStore = CommandHandler.createDemoStore CommandHandler.StorageType.InMemory

let pipeline cmd =
    cmd
    |> CommandHandler.handle eventStore
    |> List.iter ReadSide.handle

let printState (desc:string) = eventStore.GetCurrentState() |> printfn "[%s] %A" (desc.ToUpper())

type Bookies =
    CsvProvider<"/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/bookies.csv">
type Deposits =
    CsvProvider<"/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/deposits.csv">
type Withdrawals =
    CsvProvider<"/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/withdrawals.csv">
type PlaceBackBets =
    CsvProvider<"/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/placebackbets.csv">

let includedBookies = ["436a7be7-7ecc-49ee-8592-8b1309d231c7"]

[<EntryPoint>]
let main argv =

    printState "Initial"

    let bookiesPath = "/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/bookies.csv"
    let bookies = Bookies.Load(bookiesPath).Rows
    let addBookieCommands: seq<Command> =
                            bookies
                            |> Seq.filter (fun x -> (List.contains (x.Id.ToString ()) includedBookies))
                            |> Seq.map (fun x -> AddBookie { Id = BookieId x.Id; Name = x.Name })
                            
    let depositsPath = "/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/deposits.csv"
    let deposits = Deposits.Load(depositsPath).Rows
    let depositCommands =
        deposits
        |> Seq.filter (fun x -> (List.contains (x.Id.ToString ()) includedBookies))
        |> Seq.map (fun x -> MakeDeposit {
                                Id = BookieId x.Id;
                                Transaction = { Timestamp = DateTime.UtcNow; Amount = TransactionAmount (decimal x.Amount) } })

    let withdrawalsPath = "/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/withdrawals.csv"
    let withdrawals = Withdrawals.Load(withdrawalsPath).Rows
    let withdrawalCommands =
        withdrawals
        |> Seq.filter (fun x -> (List.contains (x.Id.ToString ()) includedBookies))
        |> Seq.map (fun x -> MakeWithdrawal {
                                Id = BookieId x.Id;
                                Transaction = { Timestamp = DateTime.UtcNow; Amount = TransactionAmount x.Amount } })

    let placebackbetsPath = "/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/placebackbets.csv"
    let placebackBets = PlaceBackBets.Load(placebackbetsPath).Rows
    let placeBackBetCommands =
        placebackBets
        |> Seq.filter (fun x -> (List.contains (x.BookieId.ToString ()) includedBookies))
        |> Seq.map (fun x -> PlaceBackBet {
                                Id = BookieId x.BookieId;
                                Stake =  Stake x.Stake;
                                Odds = Odds x.Odds;
                                BetId = BetId x.BetId })

    let allCommands =
        Seq.concat [addBookieCommands; depositCommands; withdrawalCommands; placeBackBetCommands]

    Seq.iter (fun x -> x |> pipeline) allCommands

    printState "After task added"
    0