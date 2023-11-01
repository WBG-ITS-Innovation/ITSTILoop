<h1>ITSTILoop</h1>

A simple "Instant Payment System" inspired by the [Mojaloop](https://mojaloop.io/)

ITSTILoop is created for easy testing purposes. Some of the testing scenarios include

<ul>
  <li>Wholesale transfers</li>
  <li>Retail transfer</li>
  <li>CBDC integrations</li>
</ul>

<h2>Components</h2>

<ul>
  <li>ITSTILoop: Main instant payment system implementation</li> 
  <li>ITSTILoopAddressLookup: A simple alias(phonenumber, email, etc) to account identifier mapper service</li>
  <li>ITSTILoopCBDCAdapter: An adapter service to integrate ITSTILoop to an Ethereum ERC20 based CBDC system</li>
  <li>ITSTILoopSampleFSP: A very simple Bank implementation with predefined-prefunded accounts, which can be used to test integrations with the ITSTILoop</li>
</ul>

<h2>Diagrams</h2>
<h3>Wholesale</h3>

![image](https://github.com/WBG-ITS-Innovation/ITSTILoop/assets/6959580/33f7c850-184c-4036-8ec0-b8df63a85914)

<h3>Retail</h3>

![image](https://github.com/WBG-ITS-Innovation/ITSTILoop/assets/6959580/33702e83-92cf-471b-834c-13b27ac22b55)

<h2>Running and Building</h2>
<h3>Building Container Images:</h3>
<code>docker compose build</code>
<h3>Running:</h3>
<code>docker compose up</code>



