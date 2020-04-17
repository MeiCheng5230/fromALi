<template>
  <div id="app">
    <router-view/>
  </div>
</template>

<script>
export default {
  name: 'App'
}
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}
html,body,#app{
	height: 100%;
}
	body,
	p,
	div,
	h1,
	ul {
		font-size: .36rem;
		padding: 0;
		margin: 0;
		box-sizing: border-box;
	}
	ul,
	li {
		list-style: none;
	}
</style>
