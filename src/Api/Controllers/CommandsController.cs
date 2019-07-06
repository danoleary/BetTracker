using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private CommandHandler.EventStore eventStore;

        public CommandsController(CommandHandler.EventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        [HttpPost("addbookie")]
        public IActionResult AddBookie([FromBody] Dtos.AddBookieDto commandDto)
        {
            return ExecuteCommand(commandDto);
        }

        [HttpPost("makedeposit")]
        public IActionResult MakeDeposit([FromBody] Dtos.MakeDepositDto commandDto)
        {
            return ExecuteCommand(commandDto);
        }

        [HttpPost("makewithdrawal")]
        public IActionResult MakeWithdrawal([FromBody] Dtos.MakeWithdrawalDto commandDto)
        {
            return ExecuteCommand(commandDto);
        }

        [HttpPost("placebackbet")]
        public IActionResult PlaceBackBet([FromBody] Dtos.PlaceBackBetDto commandDto)
        {
            return ExecuteCommand(commandDto);
        }

        [HttpPost("placelaybet")]
        public IActionResult PlaceLayBet([FromBody] Dtos.PlaceLayBetDto commandDto)
        {
            return ExecuteCommand(commandDto);
        }

        [HttpPost("placefreebet")]
        public IActionResult PlaceFreeBet([FromBody] Dtos.PlaceFreeBetDto commandDto)
        {
            return ExecuteCommand(commandDto);
        }

        [HttpPost("settlebackBet")]
        public IActionResult SettleBackBet([FromBody] Dtos.SettleBackBetDto commandDto)
        {
            return ExecuteCommand(commandDto);
        }

        [HttpPost("settlefreeBet")]
        public IActionResult SettleFreeBet([FromBody] Dtos.SettleFreeBetDto commandDto)
        {
            return ExecuteCommand(commandDto);
        }

        [HttpPost("settlelayBet")]
        public IActionResult SettleLayBet([FromBody] Dtos.SettleLayBetDto commandDto)
        {
            return ExecuteCommand(commandDto);
        }

        [HttpPost("cashoutbackbet")]
        public IActionResult CashOutBackBet([FromBody] Dtos.CashOutBackBetDto commandDto)
        {
            return ExecuteCommand(commandDto);
        }

        [HttpPost("creditbonus")]
        public IActionResult CreditBonus([FromBody] Dtos.CreditBonusDto commandDto)
        {
            return ExecuteCommand(commandDto);
        }
 
        private IActionResult ExecuteCommand(Dtos.CommandDto domainCommand)
        {
            var executionResult = CommandHandler.handle(eventStore, domainCommand);
            if(executionResult.IsOk) {
                var state = CommandHandler.getCurrentState(eventStore, domainCommand.AggregateId);
                Console.WriteLine(state);
                return Ok();
            } else {
                Console.WriteLine(executionResult.ErrorValue);
                return Conflict(executionResult.ErrorValue);
            }
        }
    }
}
