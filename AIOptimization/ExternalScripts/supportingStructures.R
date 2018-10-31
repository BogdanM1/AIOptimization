################### iteration functions
all_iterations <- function(input)
{
  return (input[input$Iteration >= 0,])
}

skip_first_iteration <- function(input)
{
  return (input[input$Iteration > 1,])
} 

###################################### marks

engOptMarks <- data.frame(
  allDataMark      = 'Population',
  bestDataMark     = 'Best solutions',
  refDataMark      = 'Reference solutions',
  functionMark     = 'All solutions'
)

engIndMarks <- data.frame(
  fitMark               = "fitted",
  xMark                 = 'Iteration',
  yMark                 = 'Value'
)


################### colors 
mycolors <- c('#b30047','#f4c242','#0962f2','#5c0099')








