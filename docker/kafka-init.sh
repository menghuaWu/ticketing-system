#!/bin/bash
# 等待 Kafka 服務啟動
sleep 5
# 建立 topic "ticket-events"
kafka-topics --create --if-not-exists --bootstrap-server kafka:9092 --replication-factor 1 --partitions 3 --topic ticket-events
# 確認 topic 已經建立
kafka-topics --list --bootstrap-server kafka:9092