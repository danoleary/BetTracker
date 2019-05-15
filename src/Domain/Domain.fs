module Domain

open System

type Transaction = {
    Timestamp : DateTime
    Amount : decimal
}

module CmdArgs =
    type AddBookie = {
        Id : Guid
        Name : string
    }

    type MakeDeposit = {
        Id : Guid
        Transaction : Transaction
    }

    type MakeWithdrawal = {
        Id : Guid
        Transaction : Transaction
    }

type Command = 
    | AddBookie of CmdArgs.AddBookie
    | MakeDeposit of CmdArgs.MakeDeposit
    | MakeWithdrawal of CmdArgs.MakeWithdrawal

type Event =
    | BookieAdded of CmdArgs.AddBookie
    | DepositMade of CmdArgs.MakeDeposit
    | WithdrawalMade of CmdArgs.MakeWithdrawal

type Bookie = {
    Id : Guid
    Name : string
    Deposits : Transaction list
    Withdrawals : Transaction list
}

type State = {
    Bookies : Bookie list
}
    with 
        static member Init = {
            Bookies = []
        }