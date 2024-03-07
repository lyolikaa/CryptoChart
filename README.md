**Task description** 
- Display the BTC/EUR market depth chart to the end user on a simple website
- Keep an audit log of every order book snapshot that is [potentially] displayed to the end user with the timestamp of when it was acquired.
- Allow the user to enter an amount of BTC that they would like to buy. Show to the user the price (quote) for his intended purchase. Update it in real-time as you continuously acquire the order book. 

**Implementation**
- API project - DTO models, API to get from 3rd part and store
- WebClient - Blazor Web App
-- Home - main page, some comment are on UI/markup
-- BuyAmount - component for user purchase 
-- SnapshotChart - Chart component
-- SnapshotTable - use it to check snapshot data
- DB level - use of EF, Code first
- Tests - Unit tests for calculations

**In-code comments**
- TODO - possible refactor for more complex logic etc.
- #OA - my comments for particular implementation

**Custom Configuraion**
- API : CryptoContext, ExchangeApiBase
- Web : APIBaseAddress 

**Start solution**
_Run from VS_
- setup API.appsettings.CryptoContext: User Id and Password
- configure VS/Rider Multiple Startup Projects: API + WebClient