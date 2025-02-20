# 🎮 Recreando clásicos: Asteroids - Unity

Este proyecto es una recreación del clásico *Asteroids* de Atari usando Unity. Se ha diseñado para ser fiel al juego original, manteniendo su jugabilidad simple pero desafiante.
Este proyecto forma parte de una serie de recreaciones de videojuegos clásicos, diseñados para aprender, experimentar y mejorar habilidades en desarrollo de juegos con Unity.

> **Objetivo**: Reproducir la experiencia del *Asteroids* original con mecánicas fieles y código optimizado.

---

## 🎥 Referencias al juego original

Estos videos ayudan a comprender los detalles del diseño y la jugabilidad original para lograr una recreación fiel.
Para entender mejor la jugabilidad y el diseño original, puedes revisar los siguientes videos:

1. [Arcade Longplay (900) Asteroids](https://www.youtube.com/watch?v=_TKiRvGfw3Q)  
2. [Asteroids 1979 Atari Mame Retro Arcade Games](https://www.youtube.com/watch?v=Dqw6xRbCgV0)  
4. [Wikipedia](https://es.wikipedia.org/wiki/Asteroids)

---

## 📌 Game design

Entorno:
- Espacio sin bordes: cuando un objeto sale por un borde, reaparece en el lado opuesto.

Entidades:
- **Nave**: controlada por el jugador, puede girar, acelerar, disparar proyectiles y usar hiperespacio.
- **Asteroides**: grandes, medianos y pequeños, se desplazan en direcciones aleatorias.
- **Proyectiles**: disparados por las naves para destruir asteroides.
- **Platillo volador** (opcional): aparece aleatoriamente y dispara al jugador.

Comportamientos:
- La nave puede moverse libremente, aplicando inercia a su movimiento.
- Los asteroides se dividen en fragmentos más pequeños al ser destruidos.
- Al eliminar todos los asteroides en pantalla, se genera una nueva oleada.
- El jugador pierde una vida si su nave es destruida.
- El juego termina cuando el jugador pierde todas sus vidas.

Interfaz:
- Se muestra la puntuación en la parte superior.
- Indica la cantidad de vidas restantes.
- Opción de iniciar una nueva partida al perder todas las vidas.

---

## 🛠️ Requisitos

- **Unity**: Versión **60000.0.32f1** o superior (recomendado).  
- **.NET 6** o superior.  
- **Editor de código** compatible con Unity (Visual Studio o VS Code).  
- **Git** (opcional, para clonar el repositorio).  

---

## 🎮 Controles del juego

| Acción         | Tecla (Jugador 1) | Tecla (Jugador 2)
|---------------|----|---|
| Girar izquierda  | `A` | `←` |
| Girar derecha   | `D` | `→` |
| Acelerar      | `W` | `↑` |
| Disparar      | `Q` | `CTRL Derecho` |
| Hiperespacio | `E` | `Shit Derecho` |
| Start | `Espacio` | `Intro` |

> Se puede incorporar el segundo jugador el iniciar la partida

## 🐟 Licencia

Este proyecto se distribuye bajo la licencia **MIT**.  
Puedes usarlo, modificarlo y distribuirlo libremente.

---

¡Diviértete desarrollando *Asteroids* y recreando clásicos! 🚀

