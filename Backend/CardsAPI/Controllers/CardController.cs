using CardsAPI.Data;
using CardsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardController : Controller
    {
        private readonly CardsDbContext cardsDbContext;
        public CardController(CardsDbContext cardsDbContext)
        {
            this.cardsDbContext = cardsDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards=await cardsDbContext.Cards.ToListAsync();
            return Ok(cards);
        }
        [HttpGet]
        [Route("{id:guid}")]
    [ActionName("GetCard")]    
        public async Task<IActionResult> GetCard([FromRoute]Guid id)
        {
            var card=await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if(card !=null)
            {
                return Ok(card);
            }
            return NotFound("Card not found");
        }
        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody]Card card)
        {
            card.Id=Guid.NewGuid();
           await cardsDbContext.Cards.AddAsync(card);
            await cardsDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCard),new {id=card.Id},card);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult>
            UpdateCard([FromRoute]Guid id, 
            [FromBody]Card card)
        {
            var existingCard = await cardsDbContext.Cards.
                FirstOrDefaultAsync(x => x.Id == id);
            if (existingCard != null)
            {
                existingCard.CardHolderName = card.CardHolderName;
                existingCard.CardNumber = card.CardNumber;
                existingCard.ExpiryMonth = card.ExpiryMonth;
                existingCard.ExpiryYear = card.ExpiryYear;
                existingCard.CVC = card.CVC;
                await cardsDbContext.SaveChangesAsync();
                return Ok(existingCard);
            }
            return NotFound("Card not found");
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute]Guid id)
        {
            var existingCard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if(existingCard!= null)
            {
                cardsDbContext.Remove(existingCard);
                await cardsDbContext.SaveChangesAsync();
                return Ok(existingCard);
            }
            return NotFound("Card not found");
        }
    }
}
