@ticket-validator
Feature: Minimum Ticket Count For Player Validator
    As a lottery ticket validator
    I want to validate the minimum ticket requirements
    So that I can ensure tickets meet the necessary criteria

    Scenario Outline: Valiate player with 0 tickets
        Given the player has <tickets> tickets
        And the minimum amount of tickets allowed is <minTickets>
        When I validate the tickets
        Then the validation should <validationResult>
        Examples:
            | tickets | minTickets | validationResult                        |
            | 0       | 1          | fail with the appropriate error message |
            | 0       | 5          | fail with the appropriate error message |
            | 0       | 0          | succeed                                 |

    Scenario Outline: Validate player with more tickets than the minimum amount allowed
        Given the player has <tickets> tickets
        And the minimum amount of tickets allowed is <minTickets>
        When I validate the tickets
        Then the validation should succeed
        Examples:
            | tickets | minTickets |
            | 1       | 0          |
            | 1       | 1          |
            | 5       | 4          |
            | 10      | 9          |

    Scenario Outline: Validate player with less tickets than the minimum amount allowed
        Given the player has <tickets> tickets
        And the minimum amount of tickets allowed is <minTickets>
        When I validate the tickets
        Then the validation should fail with the appropriate error message
        Examples:
            | tickets | minTickets |
            | 1       | 2          |
            | 4       | 5          |
            | 9       | 10         |