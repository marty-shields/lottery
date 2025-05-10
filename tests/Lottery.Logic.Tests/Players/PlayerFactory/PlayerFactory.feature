Feature: Player Factory
    As a user of the lottery
    I want to be able to create human and computer players
    So that there are players that can participate in the bingo

    Scenario Outline: CreateComputerPlayers - The correct amount of computer players are generated and stored
        Given there is configuration that sets the min computer players to 1 and max players to 10
        And the amount of computer players set to be generated is <playersToGenerate>
        When CreateComputerPlayers is called
        Then <playersToGenerate> players would have been generated
        Examples:
            | playersToGenerate |
            | 1                 |
            | 2                 |
            | 5                 |
            | 9                 |
            | 10                |

    Scenario Outline: CreateHumanPlayer - The player is created and stored
        Given a human player is set to be generated
        When CreateHumanPlayer is called
        Then the human player would have been generated