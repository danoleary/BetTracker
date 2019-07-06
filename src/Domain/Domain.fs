module Domain

open System

type TransactionAmount = TransactionAmount of decimal

type BetResult = Win | Lose

type BetId = BetId of Guid

type BetState = Settled | NotSettled | CashedOut

type BookieId = BookieId of Guid
type AggregateId = AggregateId of Guid

type Stake = Stake of decimal

type Odds = Odds of decimal

type Balance = Balance of decimal

type Winnings = Winnings of decimal

type CashOutAmount = CashOutAmount of decimal

type Bet = {
    Id : BetId
    Stake : Stake
    Odds : Odds
    State : BetState
}

module CmdArgs =
    type AddBookie = {
        BookieId : BookieId
        Name : string
    }

    type MakeDeposit = {
        Transaction : TransactionAmount
    }

    type MakeWithdrawal = {
        Transaction : TransactionAmount
    }

    type PlaceBackBet = {
        Stake : Stake
        Odds : Odds
        BetId : BetId
    }

    type PlaceFreeBet = {
        Stake : Stake
        Odds : Odds
        BetId : BetId
    }

    type SettleBackBet = {
        Result : BetResult
        BetId : BetId
    }

    type SettleFreeBet = {
        Result : BetResult
        BetId : BetId
    }

    type PlaceLayBet = {
        Stake : Stake
        Odds : Odds
        BetId : BetId
    }

    type SettleLayBet = {
        Result : BetResult
        BetId : BetId
    }

    type CashOutBackBet = {
        BetId : BetId
        CashOutAmount : CashOutAmount
    }

    type CreditBonus = {
        Amount : TransactionAmount
    }

type CommandPayload = 
    | AddBookie of CmdArgs.AddBookie
    | MakeDeposit of CmdArgs.MakeDeposit
    | MakeWithdrawal of CmdArgs.MakeWithdrawal
    | PlaceBackBet of CmdArgs.PlaceBackBet
    | PlaceFreeBet of CmdArgs.PlaceFreeBet
    | SettleBackBet of CmdArgs.SettleBackBet
    | SettleFreeBet of CmdArgs.SettleFreeBet
    | PlaceLayBet of CmdArgs.PlaceLayBet
    | SettleLayBet of CmdArgs.SettleLayBet
    | CashOutBackBet of CmdArgs.CashOutBackBet
    | CreditBonus of CmdArgs.CreditBonus

type Command = {
    AggregateId : AggregateId
    Timestamp : DateTime
    Payload : CommandPayload
}

type Event =
    | BookieAdded of CmdArgs.AddBookie
    | DepositMade of CmdArgs.MakeDeposit
    | WithdrawalMade of CmdArgs.MakeWithdrawal
    | BackBetPlaced of CmdArgs.PlaceBackBet
    | FreeBetPlaced of CmdArgs.PlaceFreeBet
    | BackBetSettled of CmdArgs.SettleBackBet
    | FreeBetSettled of CmdArgs.SettleFreeBet
    | LayBetPlaced of CmdArgs.PlaceLayBet
    | LayBetSettled of CmdArgs.SettleLayBet
    | BackBetCashedOut of CmdArgs.CashOutBackBet
    | BonusCredited of CmdArgs.CreditBonus


type Bookie = {
    Id : BookieId
    Name : string
    Balance : Balance
    Bets : Bet list
}

type State = EmptyState | Bookie of Bookie
    with 
        static member Init = EmptyState