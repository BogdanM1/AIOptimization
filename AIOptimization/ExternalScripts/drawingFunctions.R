library(plotly)
library(rgl)
library(broom)

####################### draw optimization

drawPopulation <- function(allData,bestData, 
                                   reference,
                                   iterationCondition, 
                                   graphName,htmlName,
                                   variablesCnt,objectivesCnt, 
                                   colors = mycolors,
                                   marks = engOptMarks )
{
  # read found solutions  
  if(objectivesCnt==1)
  {
    selectColumns <- c(1:(1+variablesCnt+objectivesCnt+2))
  }
  else
  {
    selectColumns <- c(1,(variablesCnt+2):(1+variablesCnt+objectivesCnt+2))
  }
  
  dataFull    <- read.table(allData, header=T,sep=",") 
  dataFull    <- iterationCondition(dataFull)  
  dataFull    <- unique( dataFull[,selectColumns] )
 
  dataBest <- read.table(bestData, header=T,sep=",")  
  dataBest <- iterationCondition(dataBest)
  dataBest <- unique( dataBest[,selectColumns])
  
  dataBest$Type    <- marks$bestDataMark
  dataFull$Type    <- marks$allDataMark
  all_data         <- rbind(dataBest,dataFull)
  types            <- all_data$Type
    
  iterations <- all_data[,1]   
 
  if((variablesCnt>1 && objectivesCnt==1) || objectivesCnt==3) 
  {
    zaxis  <- all_data[,4]  
    ID     <- all_data[,5]  
    fvalue <- all_data[,6]
  }
  if (objectivesCnt==2) 
  {
    zaxis  <- NULL  
    ID     <- all_data[,4]  
    fvalue <- all_data[,5]    
  }

  # Draw
  if(objectivesCnt>3)
  {
    # parallel coordinate system
      parcoordsdim<-list()
      for (idim in c(1:objectivesCnt))
      {
        parcoordsdim[[idim]] <- list( label = idim, values = all_data[,1+idim])
      }
        
      plotlygraph <- plot_ly(type = 'parcoords', frame = iterations,
                             line = list(color=colors[1]),
                             dimensions = parcoordsdim)    
  } 
  else 
  {
  # Cartesian coordinate system 
    plotlygraph <- plot_ly()%>%
  # draw found solutions
    add_markers(x = all_data[,2], y = all_data[,3], z =  zaxis,
                frame = iterations,
                color = types, colors = colors, 
                text =  paste('<br> fitness value: ',  fvalue,
                              '<br> ID: ',  ID))
  # draw all solutions for single object
    if(objectivesCnt==1)
    {
      plotlygraph <- plotlygraph%>%  
      add_markers(x = all_data[,2], y = all_data[,3], z =  zaxis, 
                  marker  = list(color=colors[2]),
                  opacity = 0.1,
                  name = marks$functionMark,
                  text = paste('<br> fitness value: ',  fvalue,
                                '<br> ID: ',  ID))
    }
  # draw reference solution(s)
    if(!is.null(reference))
    {
      refDataTable <- read.table(reference,sep="")
      xRef <- refDataTable[,1]
      yRef <- refDataTable[,2]
      zRef <- NULL
      if(objectivesCnt==3)
      {
        zRef <- referentDataTable[,3]
      }
      plotlygraph <- plotlygraph%>% 
      add_markers(x = xRef, y=yRef, z=zRef, 
                marker = list(color=colors[2]), name=marks$refDataMark)
    }
  }
  plotlygraph <- plotlygraph%>%
                 layout(title=graphName, showlegend=TRUE) 
  htmlwidgets::saveWidget(as_widget(plotlygraph), htmlName, selfcontained = FALSE) 
  return (  plotlygraph  )
}


#################################################### Indicators

drawIndicators <- function(input,
                           titleName,htmlName,
                           iterationCondition,
                           colors = mycolors, 
							             marks = engIndMarks)
{
  inputTable <- read.table(input, header = T, sep=",")
  inputTable <- iterationCondition(inputTable)

  i <- 1  
  iter <- inputTable[,i]
  p <- plot_ly(x = iter) 
  colornum <- length(colors)

  for(icolumn in colnames(inputTable)[-1])
  {
    i<- i+1
    p <- p%>%
    add_markers(y = inputTable[,i],text = ~paste('iteration:', iter),  
                name = icolumn, marker = list(color = colors[colornum%%i+1])) %>%
    add_lines(y = fitted(loess(inputTable[,i] ~ iter)),
              line = list(color = colors[colornum%%i+1]),
              name = paste(icolumn,marks$fitMark)) 
  }
  
  p <- p%>%
  layout(title = titleName,
           xaxis = list(title = marks$xMark),
           yaxis = list(title = marks$yMark), 
           showlegend = TRUE) 
  htmlwidgets::saveWidget(as_widget(p), htmlName, selfcontained = FALSE) 
  return (p)
}





 










