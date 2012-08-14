var global_host_id = 1;

function host(ip,port)
{
	if(ip == undefined)
		ip = "127.0.0.1";
	if(port = undefined)
		port = "12012"
	
	var can_send = false;
	var id = global_host_id;
	id++;
	
	uexSocketMgr.cbcreateTCPSocket = function(id,dataType,data){
		can_send = true;
	}
	uexSocketMgr.createTCPSocket(id);
	 
	uexSocketMgr.onData = onmessage;
	uexSocketMgr.setInetAddressAndPort(id,ip,port);
	 

	function onmessage(id,msg) {
	    var list = msg.data.split(" ");
	    var com = list[0];
	    var data = msg.data.substring(com.length).trim();
	    try {
	        var json_data = eval("(" + data + ")");
	        eval(com + "_Command(json_data)");
	    }
	    catch (exception) {
	        eval(com + "_Command(data)");
	    }
	}
	
	function send(msg) {
		if (!can_send) {
			setTimeout("send('"+msg+"')", 100);
	        return;
	    }
		uexSocketMgr.sendData(id,msg + "\r\n");
	}
	
	function command(com, data) {
	    if (data == undefined)
	        send(com);
	    else
	        send(com + " " + data);
	}
	
	function disconnect()
	{
		uexSocketMgr.uexSocketMgr(id);
	}
}

String.prototype.trim = function () {
    var str = this.replace(/^(\s|\u00A0)+/, '');
    for (var i = str.length - 1; i >= 0; i--) {
        if (/\S/.test(str.charAt(i))) {
            str = str.substring(0, i + 1);
            break;
        }
    }
    return str;
}
