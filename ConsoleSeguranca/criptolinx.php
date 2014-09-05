<?php>
	public function encodeURL($frase, $key){
		// 1 - BASE64 de [frase]
		$primeiraPalavra = base64_encode($frase);  // $this->MD5HashCode($frase);
		// 2 - Concatena [palavra] com [chave]
        $palavraConcat = $frase.$key;
		// 3 - MD5 da [palavraConcat]
		$md5PalavraConcat =  md5($palavraConcat, false);
		// 4 - Base64 de ASCII de [md5PalavraConcat]
		$segundaPalavra = base64_encode($md5PalavraConcat);
		// 5 - Concatena [primeiraParte] + ["-"] + [segundaParte]
		$retornoCalculado = $primeiraPalavra."-".$segundaPalavra;
		/*
		echo 'Primeira Parte:<br/>';
		echo '<b>'.$primeiraPalavra.'</b><br/>';
		echo 'palavraConcat:<br/>';
		echo '<b>'.$palavraConcat.'</b><br/>';
		echo '<b>'.$md5PalavraConcat.'</b><br/>';
		echo 'Segunda Parte:<br/>';
		echo '<b>'.$segundaPalavra.'</b><br/>';
		echo 'Retorno Calculado:<br/>';
		echo '<h2>'.$retornoCalculado.'</h2><br/>';
		*/
		return $retornoCalculado;
	}

	// Se der erro, retorna vazio
	public function decodeURL($palavra, $key){
		$arr = explode("-", $palavra); 
		$sDataValue = "";
		if (count($arr) == 2) {
			$primeiraParte = $arr[0];
			$segundaParte  = $arr[1];
			$sDataValue = base64_decode($primeiraParte);
			$sStoredHash =  base64_decode($segundaParte);
			$sCalculatedHash = md5($sDataValue.$key);

			if ($sStoredHash == $sCalculatedHash){
				//echo("<h2>DEU CERTO...</h2>");
			} else {
				// ERRO
				$sDataValue = "";
				//echo("<h2>O valor passado não está correto!</h2>");
			}
		}
		
		/*
		if (count($arr) == 2) {
			print_r($arr);
			echo 'PRIMEIRA PARTE DECODIFICADA (sDataValue):<br/>';
			echo '<h2>'.$sDataValue.'</h2><br/>';
			echo 'SEGUNDA PARTE DECODIFICADA (sStoredHash):<br/>';
			echo '<h2>'.$sStoredHash.'</h2><br/>';
			echo 'RETORNO ($sCalculatedHash):<br/>';
			echo '<h2>'.$sCalculatedHash.'</h2><br/>';
		}
		*/
	  return $sDataValue;
	}

	
	