module Domain

open System

type TransactionAmount = TransactionAmount of decimal

type Transaction = {
    Timestamp : DateTime
    Amount : TransactionAmount
}

type BetResult = Win | Lose

type BetId = BetId of Guid

type BetState = Settled | NotSettled

type BookieId = BookieId of Guid

type Stake = Stake of decimal

type Odds = Odds of decimal

type Balance = Balance of decimal

type Bet = {
    Id : BetId
    Stake : Stake
    Odds : Odds
    Settled : BetState
}

module CmdArgs =
    type AddBookie = {
        Id : BookieId
        Name : string
    }

    type MakeDeposit = {
        Id : BookieId
        Transaction : Transaction
    }

    type MakeWithdrawal = {
        Id : BookieId
        Transaction : Transaction
    }

    type PlaceBackBet = {
        Id : BookieId
        Stake : Stake
        Odds : Odds
        BetId : BetId
    }

    type PlaceFreeBet = {
        Id : BookieId
        Stake : Stake
        Odds : Odds
        BetId : BetId
    }

    type SettleBackBet = {
        Id : BookieId
        Result : BetResult
        BetId : BetId
    }

type Command = 
    | AddBookie of CmdArgs.AddBookie
    | MakeDeposit of CmdArgs.MakeDeposit
    | MakeWithdrawal of CmdArgs.MakeWithdrawal
    | PlaceBackBet of CmdArgs.PlaceBackBet
    | PlaceFreeBet of CmdArgs.PlaceFreeBet
    | SettleBackBet of CmdArgs.SettleBackBet

type Event =
    | BookieAdded of CmdArgs.AddBookie
    | DepositMade of CmdArgs.MakeDeposit
    | WithdrawalMade of CmdArgs.MakeWithdrawal
    | BackBetPlaced of CmdArgs.PlaceBackBet
    | FreeBetPlaced of CmdArgs.PlaceFreeBet
    | BackBetSettled of CmdArgs.SettleBackBet

type Bookie = {
    Id : BookieId
    Name : string
    Balance : Balance
    Bets : Bet list
}

type State = {
    Bookies : Bookie list
}
    with 
        static member Init = {
            Bookies = []
        }