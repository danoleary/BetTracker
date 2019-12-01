module DataImportHelpers

open Domain

let makeCommand id timestamp payload = { AggregateId = AggregateId id; Timestamp = timestamp; Payload = payload }

let createBookie id timestamp name =
    let payload = AddBookie { BookieId = BookieId id; Name = name }
    makeCommand id timestamp payload

let makeDeposit id timestamp amount =
    let payload = MakeDeposit { Amount = TransactionAmount amount }
    makeCommand id timestamp payload