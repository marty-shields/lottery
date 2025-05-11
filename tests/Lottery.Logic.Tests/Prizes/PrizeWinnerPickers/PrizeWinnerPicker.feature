Feature: Prize Winner Pickers

    Scenario Outline: Grand prize is one winner that wins 50% of the total prize
        Given the lottery has <ticketSold> tickets sold
        And the price of each ticket was 1
        And the random number generator is setup to always return 0
        And the prize payouts are set to 50% for the grand prize, 30% for the second tier, and 10% for the third tier
        When I run the grand prize picker
        Then I should have 1 grand prize winner that won <prizeAmount>
        Examples:
            | ticketSold | prizeAmount |
            | 10         | 5           |
            | 11         | 5.5         |
            | 14         | 7           |
            | 100        | 50          |

    Scenario Outline: Second tier is 10% of people taking 30% from total prize
        Given the lottery has <ticketSold> tickets sold
        And the price of each ticket was 1
        And the random number generator is setup to always return 0
        And the prize payouts are set to 50% for the grand prize, 30% for the second tier, and 10% for the third tier
        When I run the second tier prize picker
        Then I should have <winnerCount> second tier winner that won <prizeAmount>
        Examples:
            | ticketSold | prizeAmount | winnerCount |
            | 10         | 3           | 1           |
            | 14         | 4.2         | 1           |
            | 15         | 2.25        | 2           |
            | 16         | 2.4         | 2           |
            | 19         | 2.85        | 2           |
            | 20         | 3           | 2           |
            | 25         | 3.75        | 2           |
            | 45         | 3.38        | 4           |
            | 100        | 3           | 10          |

    Scenario Outline: Second tier is 20% of people taking 10% from total prize
        Given the lottery has <ticketSold> tickets sold
        And the price of each ticket was 1
        And the random number generator is setup to always return 0
        And the prize payouts are set to 50% for the grand prize, 30% for the second tier, and 10% for the third tier
        When I run the third tier prize picker
        Then I should have <winnerCount> second tier winner that won <prizeAmount>
        Examples:
            | ticketSold | prizeAmount | winnerCount |
            | 10         | 0.5         | 2           |
            | 14         | 0.47        | 3           |
            | 15         | 0.5         | 3           |
            | 16         | 0.53        | 3           |
            | 19         | 0.48        | 4           |
            | 20         | 0.5         | 4           |
            | 25         | 0.5         | 5           |
            | 45         | 0.5         | 9           |
            | 100        | 0.5         | 20          |