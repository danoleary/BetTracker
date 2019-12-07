module DataImportHelpers

open Domain

let makeCommand id timestamp payload = { AggregateId = AggregateId id; Timestamp = timestamp; Payload = payload }

let createBookie id timestamp name =
    let payload = AddBookie { BookieId = BookieId id; Name = name }
    makeCommand id timestamp payload

let makeDeposit id timestamp amount =
    let payload = MakeDeposit { Amount = TransactionAmount amount }
    makeCommand id timestamp payload

let makeWithdrawal id timestamp amount =
    let payload = MakeWithdrawal { Amount = TransactionAmount amount }
    makeCommand id timestamp payload

let placeLayBet id timestamp stake odds betId description =
    let payload = PlaceLayBet
                     {
                        Stake = Stake stake;
                        Odds = Odds odds
                        BetId = BetId betId;
                        EventDescription = EventDescription description }
    makeCommand id timestamp payload

let settleLayBet id timestamp betId result =
    let payload = SettleLayBet
                     {
                        BetId = BetId betId;
                        Result = result }
    makeCommand id timestamp payload

let placeBackBet id timestamp stake odds betId description =
    let payload = PlaceBackBet
                     {
                        Stake = Stake stake;
                        Odds = Odds odds
                        BetId = BetId betId;
                        EventDescription = EventDescription description }
    makeCommand id timestamp payload

let settleBackBet id timestamp betId result =
    let payload = SettleBackBet
                     {
                        BetId = BetId betId;
                        Result = result }
    makeCommand id timestamp payload

let placeFreeBet id timestamp stake odds betId description =
    let payload = PlaceFreeBet
                     {
                        Stake = Stake stake;
                        Odds = Odds odds
                        BetId = BetId betId;
                        EventDescription = EventDescription description }
    makeCommand id timestamp payload

let settleFreeBet id timestamp betId result =
    let payload = SettleFreeBet
                     {
                        BetId = BetId betId;
                        Result = result }
    makeCommand id timestamp payload

let creditBonus id timestamp amount =
    let payload = CreditBonus { Amount = TransactionAmount amount }
    makeCommand id timestamp payload

