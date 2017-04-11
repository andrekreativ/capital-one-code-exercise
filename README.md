# capital-one-code-exercise
Andres Martinez

# Notes
Implemented as a REST API:<br/>
GET: http://servername/api/summaries <br/>

With the following options as query parameters:<br/>
<br/>
<b>crystalBall=true</b><br/>
Includes projected transactions for the current month and future months in the same time range as the original list of all transactions.<br/>
<br/>
<b>ignoreCcPayments=true</b><br/>
Used to ignore transactions from credit card payments in the calculations. When this option is used the output includes a list with all the pairs of transactions identifid as credit card payments. Credit card payment transactions are identified as having opposite amounts and created within 24 hours of each other.<br/>
<br/>
<b>ignoreDonuts=true</b><br/>
Used to ignore merchants that sell donuts (where the merchant is "Krispy Kreme Donuts" or "DUNKIN #336784"). Per design in the given instructions this list of ignored transactions is not displayed to the user. But a future improvement could easily show it, because the list is available just not displayed.<br/>
<br/>
<b>Output:</b><br/>
[<br/>
  {<br/>
    "ignoreDonuts": true|false,<br/>
    "crystalBall": true|false,<br/>
    "ignoreCcPayments": true|false,<br/>
    "averageMonth": {},<br/>
    "months": [],<br/>
    "excludedCreditCardPayments": []<br/>
  }<br/>
]<br/>
# Compile and run:
Use .NET Core 1.1 and Visual Studio 2017.

https://www.microsoft.com/net/core#windowsvs2017

# Tests:
+ Unit tests for each component, plug-in and algorithm.
+ TODO: Add unit tests for the controller itself.
