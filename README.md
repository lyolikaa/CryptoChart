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

**In-code comments**
- TODO - possible refactor for more complex logic etc.
- #OA - my comments f~~~~or particular implementation