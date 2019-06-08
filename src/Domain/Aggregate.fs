module Aggregate

open Domain
open AddBookieHandler
open MakeDepositHandler
open MakeWithdrawalHandler
open PlaceBackBetHandler
open PlaceFreeBetHandler
open SettleBackBetHandler
open SettleFreeBetHandler
open PlaceLayBetHandler
open BookieAddedHandler
open DepositMadeHandler
open WithdrawalMadeHandler
open BackBetPlacedHandler
open FreeBetPlacedHandler
open BackBetSettledHandler
open FreeBetSettledHandler
open LayBetPlacedHandler
open SettleLayBetHandler
open LayBetSettledHandler
open Domain.CmdArgs

type Aggregate<'state, 'command, 'event> = {
    Init : 'state
    Apply: 'state -> 'event -> 'state
    Execute: 'state -> 'command -> 'event list
}

let execute state command = 
    match command with
    | AddBookie args -> handleAddBookie state args
    | MakeDeposit args -> handleMakeDeposit state args
    | MakeWithdrawal args -> handleMakeWithdrawal state args
    | PlaceBackBet args -> handlePlaceBackBet state args
    | PlaceFreeBet args -> handlePlaceFreeBet state args
    | SettleBackBet args -> handleSettleBackBet state args
    | SettleFreeBet args -> handleSettleFreeBet state args
    | PlaceLayBet args -> handlePlaceLayBet state args
    | SettleLayBet args -> handleSettleLayBet state args

let apply state event = 
    match event with
    | BookieAdded args -> applyBookieAdded state args
    | DepositMade args -> applyDepositMade state args
    | WithdrawalMade args -> applyWithdrawlMade state args
    | BackBetPlaced args -> applyBackBetPlaced state args
    | FreeBetPlaced args -> applyFreeBetPlaced state args
    | BackBetSettled args -> applyBackBetSettled state args
    | FreeBetSettled args -> applyFreeBetSettled state args
    | LayBetPlaced args -> applyLayBetPlaced state args
    | LayBetSettled args -> applyLayBetSettled state args

let aggregate = {
    Init = State.Init
    Execute = fun s c -> execute s c |> List.singleton
    Apply = apply
}