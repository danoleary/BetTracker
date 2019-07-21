namespace Domain.Api.Controllers

open Microsoft.AspNetCore.Mvc

[<Route("api/[controller]")>]
[<ApiController>]
type CommandsController (eventStore: CommandHandler.EventStore) =
    inherit ControllerBase()

    let eventStore = eventStore

    member private this.ExecuteCommand(domainCommand: CommandDtos.CommandDto): IActionResult =
        let executionResult = CommandHandler.handle eventStore domainCommand
        match executionResult with
        | Ok _ ->
            let state = CommandHandler.getCurrentState eventStore domainCommand.AggregateId
            printfn "%A" state
            this.Ok() :> IActionResult
        | Error x ->
            printfn "%A" domainCommand.AggregateId
            printfn "%A" x
            this.Conflict(x) :> IActionResult

    [<HttpPost("addbookie")>]
    member this.AddBookie([<FromBody>] commandDto:CommandDtos.AddBookieDto) =
        this.ExecuteCommand(commandDto)

    [<HttpPost("makedeposit")>]
    member this.MakeDeposit([<FromBody>] commandDto:CommandDtos.MakeDepositDto) =
        this.ExecuteCommand commandDto
    
    [<HttpPost("makewithdrawal")>]
    member this.MakeWithdrawal([<FromBody>] commandDto:CommandDtos.MakeWithdrawalDto) =
        this.ExecuteCommand commandDto
    
    [<HttpPost("placebackbet")>]
    member this.PlaceBackBet([<FromBody>] commandDto:CommandDtos.PlaceBackBetDto) =
        this.ExecuteCommand commandDto
    
    [<HttpPost("placelaybet")>]
    member this.PlaceLayBet([<FromBody>] commandDto:CommandDtos.PlaceLayBetDto) =
        this.ExecuteCommand commandDto
    
    [<HttpPost("placefreebet")>]
    member this.PlaceFreeBet([<FromBody>] commandDto:CommandDtos.PlaceFreeBetDto) =
        this.ExecuteCommand commandDto
    
    [<HttpPost("settlebackBet")>]
    member this.SettleBackBet([<FromBody>] commandDto:CommandDtos.SettleBackBetDto) =
        this.ExecuteCommand commandDto

    [<HttpPost("settlefreeBet")>]
    member this.SettleFreeBet([<FromBody>] commandDto:CommandDtos.SettleFreeBetDto) =
        this.ExecuteCommand commandDto

    [<HttpPost("settlelayBet")>]
    member this.SettleBackBet([<FromBody>] commandDto:CommandDtos.SettleLayBetDto) =
        this.ExecuteCommand commandDto

    [<HttpPost("cashoutbackbet")>]
    member this.CashOutBackBet([<FromBody>] commandDto:CommandDtos.CashOutBackBetDto) =
        this.ExecuteCommand commandDto

    [<HttpPost("creditbonus")>]
    member this.CreditBonus([<FromBody>] commandDto:CommandDtos.CreditBonusDto) =
        this.ExecuteCommand commandDto

    [<HttpGet("{id}")>]
    member this.Get(id:int) =
        let value = "value"
        ActionResult<string>(value)

    
