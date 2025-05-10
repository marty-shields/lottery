Feature: Prize Generator

    Scenario Outline: Prizes generated based on the number of tickets sold
        Given the lottery has <ticketSold> tickets sold
        And the price of each ticket was 1
        And the random number generator is setup to always return 0
        When I generate the prizes
        Then I should have 1 grand prize winner that won <grandPrizeAmount>
        And I should have <secondTierWinners> second tier winner that won <secondTierAmount>
        And I should have <thirdTierWinners> third tier winners that won <thirdTierAmount>
        And the house profit should be <houseProfit>
        Examples:
            | ticketSold | grandPrizeAmount | secondTierWinners | secondTierAmount | thirdTierWinners | thirdTierAmount | houseProfit |
            | 10         | 5                | 1                 | 3                | 2                | 0.5             | 1           |
            | 11         | 5.5              | 1                 | 3.3              | 2                | 0.55            | 1.1         |
            | 14         | 7                | 1                 | 4.2              | 3                | 0.47            | 1.39        |
            | 100        | 50               | 10                | 3                | 20               | 0.5             | 10          |