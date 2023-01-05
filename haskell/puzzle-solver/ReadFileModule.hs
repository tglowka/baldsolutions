module ReadFileModule where

{-
  Description: The 'readPuzzle' function loads the puzzle from file.
  Arguments:
  - String - the name of the file
  Returns:
  - IO [String] - the loaded puzzle
-}

readPuzzle :: String -> IO [String]
readPuzzle filename = do
  contents <- readFile filename
  let puzzle = read contents :: [String]
  return puzzle
