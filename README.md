# Lottery Project

Welcome to the Lottery Project! This repository contains the implementation of a lottery system.

## Features

- Random number generation for lottery draws.
- Configurable lottery rules that are validated on startup
- Dependency Injection
- BDD tests
- Extensible prize draw generation

## Possible Changes
- Improve testing around prize winner picker
    - The prize generator tests are more like an integration or end to end test testing the full prize generation rather than truly mocking out the winner pickers.
- Refactor the presentation level to make it more readable
- Grouping and ordering of the players when outputting the tiered winners relies upon the player names being very specific.
- Logging & metrics inclusion
    - Logging in places such as logging exceptions as errors to make the system easy to debug upon issues
    - Metrics and the use of traces and spans could be used to drive insights around how the players are behaving with the lottery system. Examples such as tickets purchased so you can get averages
- Using normal unit testing instead of BDD?
- Persistence
    - Currently there is none, I could have added a new project dedicated to storing player, ticket and game data.
    - Although balance is checked when purchasing tickets, the balance is never updated to remove upon tickets purchased or when player has won
    - Because of the above, it means you can always only run the console application once and it requires a restart to run the lottery again
- Concurrency / Performance
    - Due to the scope there was not much chance to use asynchronous methods. These would be used over synchronous methods whenever possible (such as data storage)