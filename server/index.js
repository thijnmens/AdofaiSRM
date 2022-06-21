const request = require('request');
const tmi = require('tmi.js');
const WebSocketServer = require('ws').Server;
const fs = require('fs');
const path = require('path');
const config = JSON.parse(fs.readFileSync(path.join(process.cwd(), './config.json')));
const wss = new WebSocketServer({ port: 666 });

wss.on('connection', function connection(ws) {
	ws.isAlive = true;
	ws.on('pong', heartbeat);
	ws.send('=Connection Success!');
});

const client = new tmi.Client({
	options: { debug: true },
	identity: {
		username: 'AdofaiSRM',
		password: config.twitchToken, // https://twitchapps.com/tmi/
	},
	channels: config.twitchChannels,
});

client.on('message', (channel, tags, message, self) => {
	// Ignore echoed messages.
	if (self || !message.startsWith('!')) return;

	const args = message.slice(1).split(' ');
	const command = args.shift().toLowerCase();

	switch (command.toLowerCase()) {
		case 'ping':
			client.ping().then((data) => client.say(channel, `Pong! | ${data * 1000}ms`));
			break;

		case 'srm':
			if (args.length === 0) {
				client.say(
					channel,
					`@${tags.username}, >srm requires at least 1 argument | >srm {ID}`
				);
				break;
			}
			const levelID = args[0].split(':');
			switch (levelID[0]) {
				default:
				case 'gg':
					break;

				case 'steam':
					break;
			}
			request(
				`https://adofai.gg:9200/api/v1/levels/${args[0]}`,
				{ json: true },
				(err, res, body) => {
					if (err) {
						return console.error(err);
					}
					wss.clients.forEach((ws) => {
						ws.send(res.body.workshop);
					});
					if (res.body.workshop === null) {
						client.say(
							channel,
							`@${tags.username},"${res.body.title}" is not uploaded to the steam workshop, non-workshop levels are not yet supported`
						);
					} else {
						client.say(
							channel,
							`@${tags.username}, I've successfully added "${res.body.title}" to the queue!`
						);
					}
				}
			);
			break;

		default:
			break;
	}
});

function heartbeat() {
	this.isAlive = true;
}

const interval = setInterval(function ping() {
	wss.clients.forEach(function each(ws) {
		if (ws.isAlive === false) return ws.terminate();

		ws.isAlive = false;
		ws.ping();
	});
}, 10000);

wss.on('close', function close() {
	clearInterval(interval);
});

client.connect();
