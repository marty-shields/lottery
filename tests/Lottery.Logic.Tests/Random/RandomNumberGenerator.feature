Feature: Random Number Generator
    As a user of the lottery system
    I want to generate random numbers within a specified range
    So that I can use them in lottery logic as well as generating a random number of computer generated players.

    Scenario: Generate a random number within a valid range
        Given the minimum number is 1
        And the maximum number is 100
        When I generate a random number
        Then the result should be between 1 and 99 inclusive

    Scenario: Generate a random number with the same min and max value
        Given the minimum number is 42
        And the maximum number is 42
        When I generate a random number
        Then the result should always be 42

    Scenario: Generate a random number with invalid range
        Given the minimum number is 100
        And the maximum number is 1
        When I attempt to generate a random number
        Then an exception should be thrown indicating an invalid range
