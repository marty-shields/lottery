Feature: Player Generator
    As a user of the lottery system
    I want to generate new players with specific name and balance
    So that they can participate in the lottery

    Scenario: Generate a new player with valid data
        Given I have a player name 'John Doe'
        And I have a starting balance setup in configuration as 10
        When the player is generated
        Then the player should be created successfully
        And the player name should be 'John Doe'
        And the player balance should be 10