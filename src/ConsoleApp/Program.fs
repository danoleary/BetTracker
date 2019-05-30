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

[<EntryPoint>]
let main argv =

    printState "Initial"

    let bookiesPath = "/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/bookies.csv"
    let bookies = Bookies.Load(bookiesPath).Rows
    let addBookieCommands = bookies |> Seq.map (fun x -> AddBookie { Id = x.Id; Name = x.Name })
    Seq.iter (fun x -> x |> pipeline) addBookieCommands

    let depositsPath = "/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/deposits.csv"
    let deposits = Deposits.Load(depositsPath).Rows
    let depositCommands =
        deposits
        |> Seq.map (fun x -> MakeDeposit {
                                Id = x.Id;
                                Transaction = { Timestamp = DateTime.UtcNow; Amount = x.Amount } })
    Seq.iter (fun x -> x |> pipeline) depositCommands

    let withdrawalsPath = "/Users/danieloleary/Documents/Github/BetTracker/src/ConsoleApp/data/withdrawals.csv"
    let withdrawals = Withdrawals.Load(withdrawalsPath).Rows
    let withdrawalCommands =
        withdrawals
        |> Seq.map (fun x -> MakeWithdrawal {
                                Id = x.Id;
                                Transaction = { Timestamp = DateTime.UtcNow; Amount = x.Amount } })
    Seq.iter (fun x -> x |> pipeline) withdrawalCommands

    printState "After task added"
    0