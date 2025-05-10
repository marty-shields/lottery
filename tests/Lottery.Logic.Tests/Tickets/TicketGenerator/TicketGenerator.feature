Feature: Ticket Generator
    As a lottery player
    I want to generate lottery tickets
    So that I can play the lottery

    Scenario Outline: Player generates tickets successfully without errors
        Given the player has a balance of <balance> and is requesting <ticketCount> tickets
        And the validation allows between <minTickets> and <maxTickets> tickets with a ticket price of <ticketPrice> with <ticketCount> tickets
        When the player generates the tickets requested
        Then the maximum balance should have been checked 1 times
        And the validation should have been called with the ticket count of <ticketCount>
        And <ticketCount> tickets should be generated successfully
        Examples:
            | balance | ticketCount | minTickets | maxTickets | ticketPrice |
            | 100     | 1           | 1          | 10         | 10          |
            | 100     | 5           | 1          | 10         | 10          |
            | 100     | 10          | 1          | 10         | 10          |

    Scenario: Player generates more tickets than their balance successfully
        Given the player has a balance of 1 and is requesting 2 tickets
        And the validation allows between 1 and 10 tickets with a ticket price of 1 with 1 tickets
        When the player generates the tickets requested
        Then the maximum balance should have been checked 1 times
        And 1 tickets should be generated successfully
        And the validation should have been called with the ticket count of 1

    Scenario: Player has 0 balance returns exception
        Given the player has a balance of 0 and is requesting 1 tickets
        And the validation allows between 1 and 10 tickets with a ticket price of 1 with 1 tickets
        When the player generates the tickets requested
        Then the maximum balance should have been checked 1 times
        And the validation should not have been called
        And an exception should be thrown of type 'InvalidOperationException' and Message 'Player does not have enough balance to generate tickets.'

    Scenario: Player balance is not enough for 1 ticket returns exception
        Given the player has a balance of 1 and is requesting 1 tickets
        And the validation allows between 1 and 10 tickets with a ticket price of 2 with 1 tickets
        When the player generates the tickets requested
        Then the maximum balance should have been checked 1 times
        And the validation should not have been called
        And an exception should be thrown of type 'InvalidOperationException' and Message 'Player does not have enough balance to generate tickets.'

    Scenario: Player requests more tickets than is allowed by configuration returns exception
        Given the player has a balance of 10 and is requesting 10 tickets
        And the validation fails with between 1 and 2 tickets with a ticket price of 1 with 10 tickets
        When the player generates the tickets requested
        Then the maximum balance should have been checked 1 times
        And the validation should have been called with the ticket count of 10
        And an exception should be thrown of type 'InvalidOperationException' and Message 'Ticket generation failed due to validation errors.'
