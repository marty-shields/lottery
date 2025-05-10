@ticket-validator
Feature: Ticket Validator Service
    As a lottery ticket validator
    I want to validate lottery tickets
    So that I can ensure they are valid and not expired

    Scenario Outline: Validate a person with all valid tickets
        Given the player has <ticketCount> tickets
        And there are validators that would mark the tickets as valid
        When the player tries to validate the tickets
        Then the result should come back as valid with no errors
        And each validator should be called once
        Examples:
            | ticketCount |
            | 0           |
            | 1           |
            | 2           |

    Scenario Outline: Validate a person with some invalid tickets
        Given the player has <ticketCount> tickets
        And there are validators that would mark the tickets as invalid
        When the player tries to validate the tickets
        Then the result should come back as invalid with errors
        And each validator should be called once
        Examples:
            | ticketCount |
            | 0           |
            | 1           |
            | 2           |
