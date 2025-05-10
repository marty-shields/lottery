@ticket-validator
Feature: Maximum Ticket Count For Player Validator
    As a lottery ticket validator
    I want to validate the maximum ticket requirements
    So that I can ensure tickets meet the necessary criteria

    Scenario Outline: Valiate player with 0 tickets
        Given the player has <tickets> tickets
        And the maximum amount of tickets allowed is <maxTickets>
        When I validate the tickets
        Then the validation should <validationResult>
        Examples:
            | tickets | maxTickets | validationResult |
            | 0       | 1          | succeed          |
            | 0       | 0          | succeed          |

    Scenario Outline: Validate player with less tickets than the maximum amount allowed
        Given the player has <tickets> tickets
        And the maximum amount of tickets allowed is <maxTickets>
        When I validate the tickets
        Then the validation should succeed
        Examples:
            | tickets | maxTickets |
            | 1       | 1          |
            | 1       | 2          |
            | 10      | 11         |

    Scenario Outline: Validate player with more tickets than the maximum amount allowed
        Given the player has <tickets> tickets
        And the maximum amount of tickets allowed is <maxTickets>
        When I validate the tickets
        Then the validation should fail with the appropriate error message
        Examples:
            | tickets | maxTickets |
            | 2       | 1          |
            | 5       | 1          |
            | 10      | 9          |